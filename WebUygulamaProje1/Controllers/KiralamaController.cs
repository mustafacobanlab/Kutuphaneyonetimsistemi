using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitaplarRepository _kitaplarRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitaplarRepository kitaplarRepository,IWebHostEnvironment webHostEnvironment)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitaplarRepository = kitaplarRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            List<Kiralama> objKiralamaList = _kiralamaRepository.GetAll(includeProps:"Kitaplar").ToList();
            

            return View(objKiralamaList);
        }

        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitaplarList = _kitaplarRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.KitapAdi,
                    Value = k.Id.ToString()

                }
                );
            ViewBag.KitaplarList = KitaplarList;

            if (id == null || id == 0)
            {
                // ekle
                return View();
            }
            else
            {
                // guncelleme

                Kiralama? KiralamaVt = _kiralamaRepository.Get(u => u.Id == id);
                if (KiralamaVt == null) { return NotFound(); }
                return View(KiralamaVt);
            }
        }

        [HttpPost]
        public IActionResult EkleGuncelle(Kiralama kiralama)
        {
            if (ModelState.IsValid)
            {
                

                if (kiralama.Id == 0)
                {
                    _kiralamaRepository.Ekle(kiralama);
                    TempData["basarili"] = "Kiralama Kaydı Başarıyla Oluşturuldu.";
                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);
                        TempData["basarili"] = "Kiralama Kaydı Başarılıyla Güncellendi.";
                }
                    
                _kiralamaRepository.Kaydet(); //SaveChanges yapmazsanız bilgiler veritabanına eklenmez.
                
                return RedirectToAction("Index");
            }
            return View();
        }

        
        // GET ACTION
        public IActionResult Sil(int? id)
        {
            IEnumerable<SelectListItem> KitaplarList = _kitaplarRepository.GetAll()
               .Select(k => new SelectListItem
               {
                   Text = k.KitapAdi,
                   Value = k.Id.ToString()

               }
               );
            ViewBag.KitaplarList = KitaplarList;


            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kiralama? KiralamaVt = _kiralamaRepository.Get(u => u.Id == id);
            if (KiralamaVt == null) { return NotFound(); }
            return View(KiralamaVt);
        }

        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {
            Kiralama? kiralama = _kiralamaRepository.Get(u => u.Id == id);
            if (kiralama == null)
            {
                return NotFound(); 

                }
            _kiralamaRepository.Sil(kiralama);
            _kiralamaRepository.Kaydet();
            TempData["basarili"] = "Kiralama Kaydı Başarıyla Silindi.";
            return RedirectToAction("Index");
        }
    }
}
