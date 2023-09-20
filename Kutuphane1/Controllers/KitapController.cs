using Kutuphane1.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUygulamaProje1.Data;
using WebUygulamaProje1.Models;

namespace WebUygulamaProje1.Controllers
{
    
    public class KitapController : Controller
    {
        private readonly IKitapRepository _kitapRepo;
        private readonly IKitapTuruRepository _kitapTuruRepository;
        private readonly IWebHostEnvironment _webHostEnviroment;

        public KitapController(IKitapRepository context, IKitapTuruRepository kitapTuruRepository, IWebHostEnvironment webHostEnviroment)
        {
            _kitapRepo = context;
            _kitapTuruRepository = kitapTuruRepository;
            _webHostEnviroment = webHostEnviroment;
        }
        [Authorize(Roles = "Admin,Ogrenci")]
        public IActionResult Index()
        {
            List<Kitap> objKitapList = _kitapRepo.GetAll(includeProps:"KitapTuru").ToList();
            return View(objKitapList);
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(int? id)
        {
            IEnumerable<SelectListItem> KitapTuruList = _kitapTuruRepository.GetAll()
               .Select(k => new SelectListItem
               {
                   Text = k.Ad,
                   Value = k.Id.ToString()
               });
            ViewBag.KitapTuruList = KitapTuruList;
            if (id == null || id == 0)
            {
                //ekle
                return View();
            }
            else
            {
                //güncelle
                Kitap? kitapVt = _kitapRepo.Get(u => u.Id == id);  //Expression<Func<T, bool>> filtre
                if (kitapVt == null)
                { return NotFound(); }
                return View(kitapVt);
            }

        }
        [HttpPost]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult EkleGuncelle(Kitap kitap, [FromForm] IFormFile file)
        {
            //_uygulamaDbContext.kitapTurleri.Add(kitapTuru);
            //_uygulamaDbContext.SaveChanges();
            //return RedirectToAction("Index", "KitapTuru"); frontend @section kısmını kullanarak kontrol yapmak için
            //var errosrs = ModelState.Values.SelectMany(x => x.Errors); //kodda hata varsa yakalar.

            if (ModelState.IsValid)
            {


                string wwwRootPath = _webHostEnviroment.WebRootPath;
                string kitapPath = Path.Combine(wwwRootPath, @"images");

                if (file != null)
                {
                    using (var fileStream = new FileStream(Path.Combine(kitapPath, file.FileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                        kitap.ResimUrl = @"\images\" + file.FileName;
                    }
                }
               

                if (kitap.Id == 0)
                {
                    _kitapRepo.Ekle(kitap);
                    TempData["basarili"] = "Kitap ekleme işlemi başarıyla gerçekleştirildi.";
                }
                else
                {
                    _kitapRepo.Guncelle(kitap);
                    TempData["basarili"] = "Kitap Güncelleme işlemi başarıyla gerçekleştirildi.";
                }


                _kitapRepo.Kaydet();
                return RedirectToAction("Index", "Kitap");
            }
            return View();
            //BU KISIM BACKAND VALİDATİON (KONTROL İÇİN)
        }
        /* public IActionResult Guncelle(int? id)
         {
             if (id == null || id == 0)
             {
                 return NotFound();
             }
             Kitap? kitapVt = _kitapRepo.Get(u => u.Id == id);  //Expression<Func<T, bool>> filtre
             if (kitapVt == null)
             { return NotFound(); }
             return View(kitapVt);
         }
        /* [HttpPost]
         public IActionResult Guncelle(Kitap kitap)
         {

             if (ModelState.IsValid)
             {
                 _kitapRepo.Guncelle(kitap);
                 _kitapRepo.Kaydet();
                 TempData["basarili"] = "Kitap türü güncelleme başarıyla gerçekleştirildi.";
                 return RedirectToAction("Index", "Kitap");
             }
             return View();
             //BU KISIM BACKAND VALİDATİON (KONTROL İÇİN)
         }*/
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult Sil(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kitap? kitapVt = _kitapRepo.Get(u => u.Id == id);
            if (kitapVt == null)
            { return NotFound(); }
            return View(kitapVt);
        }
        [HttpPost, ActionName("Sil")]
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult SilPOST(int? id)
        {

            Kitap? kitap = _kitapRepo.Get(u => u.Id == id);
            if (kitap == null)
            {
                return NotFound();
            }
            _kitapRepo.Sil(kitap);
            _kitapRepo.Kaydet();
            TempData["basarili"] = "Kitap başarıyla silindi.";
            return RedirectToAction("Index", "Kitap");


            //BU KISIM BACKAND VALİDATİON (KONTROL İÇİN)
        }
        [Authorize(Roles = UserRoles.Role_Admin)]
        public IActionResult View(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Kitap? kitapVt = _kitapRepo.Get(u => u.Id == id);
            if (kitapVt == null)
            { return NotFound(); }
            return View(kitapVt);
        }
    }
}
