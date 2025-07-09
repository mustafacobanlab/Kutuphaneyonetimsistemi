using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Models;
using WebUygulamaProje1.Utility;

namespace WebUygulamaProje1.Controllers
{
    
    public class KitaplarController : Controller
    {
        private readonly IKitaplarRepository _KitaplarRepository;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        public readonly IWebHostEnvironment _webHostEnvironment;
        public KitaplarController(IKitaplarRepository kitaplarRepository, IKitapTuruRepository kitapTuruRepository,IWebHostEnvironment webHostEnvironment)
        {
            _KitaplarRepository = kitaplarRepository;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = "Admin,Ogrenci")]
        
        public IActionResult Index()
        {
            List<Kitaplar> objKitaplarList = _KitaplarRepository.GetAll(includeProps:"KitapTuru").ToList();
            

            return View(objKitaplarList);
        }

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll()
                .Select(k => new SelectListItem
                {
                    Text = k.Ad,
                    Value = k.Id.ToString()

                }
                );
            ViewBag.KitapTuruList = KitapTuruList;

            if (id == null || id == 0)
            {
                // ekle
                return View();
            }
            else
            {
                // guncelleme

                Kitaplar? KitaplarVt = _KitaplarRepository.Get(u => u.Id == id);
                if (KitaplarVt == null) { return NotFound(); }
                return View(KitaplarVt);
            }
        }

        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(Kitaplar kitaplar,IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                if (file != null)
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    string kitapPath = Path.Combine(wwwRootPath, @"img");

                    using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    kitaplar.ResimUrl = @"\img\" + file.FileName;
                }
               

                if (kitaplar.Id == 0)
                {
                    _KitaplarRepository.Ekle(kitaplar);
                    TempData["basarili"] = "Kitap Başarılıyla Oluşturuldu.";
                }
                else
                {
                    _KitaplarRepository.Guncelle(kitaplar);
                        TempData["basarili"] = "Kitap Başarılıyla Güncellendi.";
                }
                    
                _KitaplarRepository.Kaydet(); //SaveChanges yapmazsanız bilgiler veritabanına eklenmez.
                
                return RedirectToAction("Index");
            }
            return View();
        }

        /*
        public IActionResult Guncelle(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kitaplar? KitaplarVt = _KitaplarRepository.Get(u=>u.Id==id);
            if (KitaplarVt == null) { return NotFound(); }
            return View(KitaplarVt);
        }
        */
        /*
        [HttpPost]
        public IActionResult Guncelle(Kitaplar kitaplar)
        {
            if (ModelState.IsValid)
            {
                _KitaplarRepository.Guncelle(kitaplar);
                _KitaplarRepository.Kaydet(); //SaveChanges yapmazsanız bilgiler veritabanına eklenmez.
                TempData["basarili"] = "Kitap Başarılıyla Güncellendi.";
                return RedirectToAction("Index");
            }
            return View();
        }
        */
        // GET ACTION

        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kitaplar? KitaplarVt = _KitaplarRepository.Get(u => u.Id == id);
            if (KitaplarVt == null) { return NotFound(); }
            return View(KitaplarVt);
        }

        [HttpPost, ActionName("Sil")]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult SilPOST(int? id)
        {
            Kitaplar? Kitaplar = _KitaplarRepository.Get(u => u.Id == id);
            if (Kitaplar == null)
            {
                return NotFound(); 

                }
            _KitaplarRepository.Sil(Kitaplar);
            _KitaplarRepository.Kaydet();
            TempData["basarili"] = "Kitap Başarılıyla Silindi.";
            return RedirectToAction("Index");
        }
    }
}
