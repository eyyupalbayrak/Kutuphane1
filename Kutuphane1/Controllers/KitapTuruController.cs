using Kutuphane1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebUygulamaProje1.Data;
using WebUygulamaProje1.Models;

namespace WebUygulamaProje1.Controllers
{
    [Authorize(Roles = UserRoles.Role_Admin)]
    public class KitapTuruController : Controller
    {
        private readonly IKitapTuruRepository _kitapTuruRepo;

        public KitapTuruController(IKitapTuruRepository context)
        {
            _kitapTuruRepo = context;
        }
        public IActionResult Index()
        {
            List<KitapTuru> objKitapTuruList = _kitapTuruRepo.GetAll().ToList();
            return View(objKitapTuruList);
        }

        public IActionResult Ekle()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Ekle(KitapTuru kitapTuru)
        {
            //_uygulamaDbContext.kitapTurleri.Add(kitapTuru);
            //_uygulamaDbContext.SaveChanges();
            //return RedirectToAction("Index", "KitapTuru"); frontend @section kısmını kullanarak kontrol yapmak için
            if (ModelState.IsValid)
            {
                _kitapTuruRepo.Ekle(kitapTuru);
                _kitapTuruRepo.Kaydet();
                TempData["basarili"] = "Kitap türü ekleme işlemi başarıyla gerçekleştirildi.";
                return RedirectToAction("Index", "KitapTuru");
            }
            return View();
            //BU KISIM BACKAND VALİDATİON (KONTROL İÇİN)
        }
        public IActionResult Guncelle(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            KitapTuru? kitapTuruVt = _kitapTuruRepo.Get(u=>u.Id==id);  //Expression<Func<T, bool>> filtre
            if (kitapTuruVt == null)
            { return NotFound(); }
            return View(kitapTuruVt);
        }
        [HttpPost]
        public IActionResult Guncelle(KitapTuru kitapTuru)
        {
           
            if (ModelState.IsValid)
            {
                _kitapTuruRepo.Guncelle(kitapTuru);
                _kitapTuruRepo.Kaydet();
                TempData["basarili"] = "Kitap türü güncelleme başarıyla gerçekleştirildi.";
                return RedirectToAction("Index", "KitapTuru");
            }
            return View();
            //BU KISIM BACKAND VALİDATİON (KONTROL İÇİN)
        }
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            KitapTuru? kitapTuruVt = _kitapTuruRepo.Get(u => u.Id == id);
            if (kitapTuruVt == null)
            { return NotFound(); }
            return View(kitapTuruVt);
        }
        [HttpPost, ActionName("Sil")]
        public IActionResult SilPOST(int? id)
        {

            KitapTuru? kitapTuru = _kitapTuruRepo.Get(u => u.Id == id);
            if (kitapTuru == null)
            {
                return NotFound();
            }
            _kitapTuruRepo.Sil(kitapTuru);
           _kitapTuruRepo.Kaydet();
            TempData["basarili"] = "Kitap türü başarıyla silindi.";
            return RedirectToAction("Index", "KitapTuru");


            //BU KISIM BACKAND VALİDATİON (KONTROL İÇİN)
        }
    }
}
