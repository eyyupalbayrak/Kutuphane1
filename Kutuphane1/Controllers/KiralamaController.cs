using Kutuphane1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Data;
using WebUygulamaProje1.Models;

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KiralamaController : Controller
    {
        private readonly IKiralamaRepository _kiralamaRepository;
        private readonly IKitapRepository _kitapRepo;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public KiralamaController(IKiralamaRepository kiralamaRepository, IKitapRepository kitapRepository, IWebHostEnvironment webHostEnviroment)
        {
            _kiralamaRepository = kiralamaRepository;
            _kitapRepo = kitapRepository;
            _webHostEnviroment = webHostEnviroment;
        }
        public IActionResult Index()
        {
            List<Kiralama> objKiralamapList = _kiralamaRepository.GetAll(includeProps:"Kitap").ToList();
            return View(objKiralamapList);
        }

        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepo.GetAll()
               .Select(k => new SelectListItem
               {
                   Text = k.KitapAdi,
                   Value = k.Id.ToString()
               });
            ViewBag.KitapList = KitapList;
            if (id == null || id == 0)
            {
                //ekle
                return View();
            }
            else
            {
                //güncelle
                Kiralama? kiralamaVt = _kiralamaRepository.Get(u => u.Id == id);  //Expression<Func<T, bool>> filtre
                if (kiralamaVt == null)
                { return NotFound(); }
                return View(kiralamaVt);
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
                    TempData["basarili"] = "Kiralama işlemi başarıyla gerçekleştirildi.";
                }
                else
                {
                    _kiralamaRepository.Guncelle(kiralama);
                    TempData["basarili"] = "Kiralama Güncelleme işlemi başarıyla gerçekleştirildi.";
                }


                _kiralamaRepository.Kaydet();
                return RedirectToAction("Index", "Kiralama");
            }
            return View();
            //BU KISIM BACKAND VALİDATİON (KONTROL İÇİN)
        }
        public IActionResult Sil(int? id)
        {
            IEnumerable<SelectListItem> KitapList = _kitapRepo.GetAll()
               .Select(k => new SelectListItem
               {
                   Text = k.KitapAdi,
                   Value = k.Id.ToString()
               });
            ViewBag.KitapList = KitapList;
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kiralama? kirakalamaVt = _kiralamaRepository.Get(u => u.Id == id);
            if (kirakalamaVt == null)
            { return NotFound(); }
            return View(kirakalamaVt);
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
            TempData["basarili"] = "Kiralama başarıyla silindi.";
            return RedirectToAction("Index", "Kiralama");


            //BU KISIM BACKAND VALİDATİON (KONTROL İÇİN)
        }
        
    }
}
