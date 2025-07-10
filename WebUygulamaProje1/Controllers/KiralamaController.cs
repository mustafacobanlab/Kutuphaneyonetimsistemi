using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Identity.Client;
using WebUygulamaProje1.Migrations;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility; // UserRoles gibi yardımcı sınıflarınızın burada olduğundan emin olun.

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitaplarRepository _kitaplarRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IApplicationUserRepository _applicationUserRepository;

        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitaplarRepository kitaplarRepository, IWebHostEnvironment webHostEnvironment, IApplicationUserRepository applicationUserRepository)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitaplarRepository = kitaplarRepository;
            _webHostEnvironment = webHostEnvironment;
            _applicationUserRepository = applicationUserRepository;
        }

        public IActionResult Index()


        {
            // Index sayfasında Kiralama listesini çekerken hem Kitaplar hem de ApplicationUser'ı dahil et.
            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps: "Kitaplar,ApplicationUser").ToList();
            return View(objKiralamaList);
        }

       

        public IActionResult EkleGuncelle(int? id)
        {
            // Dropdown listelerini dolduran yardımcı metodu çağırın.
            DoldurViewBagListeleri();

            if (id == null || id == 0)
            {
                // Yeni kiralama
                return View(new Kiralama());
            }
            else
            {
                // Güncelleme
                // Güncelleme için kiralama kaydını çekerken Kitaplar ve ApplicationUser'ı dahil edin.
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id, includeProps: "Kitaplar,ApplicationUser");
                if (kiralamaVt == null) { return NotFound(); }
                return View(kiralamaVt);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma için önerilir
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
            // Eğer ModelState geçerli değilse
            if (!ModelState.IsValid) // BURASI ÖNEMLİ: Hata varsa!
            {
                // View'ı tekrar göstermeden önce dropdown listelerini yeniden doldur.
                // Aksi takdirde, dropdown'lar boş gelir ve kullanıcı doğru seçim yapamaz.
                DoldurViewBagListeleri();
                return View(kiralama); // Geçersiz model ile View'a geri dön ki hatalar gösterilsin.
            }

            // ModelState geçerliyse kayıt işlemlerine devam et.
            if (kiralama.Id == 0)
            {
                // Yeni Kiralama Oluşturma
                Kitaplar? kiralananKitap = _kitaplarRepository.Get(u => u.Id == kiralama.KitaplarId);
                if (kiralananKitap.stock > 0)
                {
                    kiralananKitap.stock = kiralananKitap.stock - 1;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Bu kitap için yeterli stok bulunmamaktadır.");
                    DoldurViewBagListeleri(); // Dropdownları tekrar doldur
                    return View(kiralama);
                }

                // Yeni kiralama oluşturulduğunda oluşturma tarihini set et
                kiralama.olusturma = DateTime.Now;
                kiralama.bitis = DateTime.Now.AddDays(14);

                _kiralamaRepository.Ekle(kiralama);
                TempData["basarili"] = "Kiralama Kaydı Başarıyla Oluşturuldu.";
            }
            else
            {
                // Mevcut Kiralama Güncelleme
                _kiralamaRepository.Guncelle(kiralama);
                TempData["basarili"] = "Kiralama Kaydı Başarıyla Güncellendi.";
            }

            _kiralamaRepository.Kaydet(); // SaveChanges yapmazsanız bilgiler veritabanına eklenmez.
           // toplam gün farkı (tam sayıya çevrildi)
            return RedirectToAction("Index");
        }

        // Dropdown listelerini dolduran yardımcı metod
        private void DoldurViewBagListeleri()
        {
            IEnumerable<SelectListItem> applicationUserList = _applicationUserRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.Ogrencino.ToString()+" "+k.AdSoyad,
                Value = k.Id.ToString()
            });
            ViewBag.applicationUserList = applicationUserList;

            IEnumerable<SelectListItem> KitaplarList = _kitaplarRepository.GetAll().Select(k => new SelectListItem
            {
                Text = k.KitapAdi,
                Value = k.Id.ToString()
            });
            ViewBag.KitaplarList = KitaplarList;
        }

        // GET ACTION Sil
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            // Silme view'ında da Kitaplar ve ApplicationUser'ı dahil edin
            Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id, includeProps: "Kitaplar,ApplicationUser");
            if (kiralamaVt == null) { return NotFound(); }
            return View(kiralamaVt);
        }

        [HttpPost, ActionName("Sil")]
        [ValidateAntiForgeryToken] // CSRF saldırılarına karşı koruma için önerilir
        public IActionResult SilPOST(int? id)
        {
            Kiralama? kiralama = _kiralamaRepository.Get(u => u.Id == id);
            if (kiralama == null)
            {
                return NotFound();
            }

            Kitaplar? kiralananKitap = _kitaplarRepository.Get(u => u.Id == kiralama.KitaplarId);
            kiralananKitap.stock = kiralananKitap.stock + 1;

            _kiralamaRepository.Sil(kiralama);
            _kiralamaRepository.Kaydet();

            TempData["basarili"] = "Kiralama Kaydı Başarıyla Silindi.";
            return RedirectToAction("Index");
        }
    }
}