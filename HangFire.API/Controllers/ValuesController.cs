using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using HangFire.API.Context;
using Microsoft.AspNetCore.Mvc;

namespace HangFire.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        OtelContext db = new OtelContext();
        // GET api/values
        [HttpGet]
        [Route("otelrandevu")]
        public ActionResult<IEnumerable<string>> OtelRandevu()
        {
           
            var jobId = BackgroundJob.Enqueue(() => ProcessFireAndForgetJob());
          //  var otelRandevu = BackgroundJob.Schedule(() => OtelRezervasyonYap(), TimeSpan.FromDays(4));

            return new string[] { "value1", "value2" };
        }
        [HttpGet]
        [Route("rezervasyon")]
        public ActionResult OdaRezervasyon()
        {
           
            Rezervasyon rezervasyon=new Rezervasyon
            {
                CikisTarihi = DateTime.Now.AddDays(3),
                GirisTarihi = DateTime.Now.AddDays(1),
                Fiyat = 500,
                OdaId = 5,
                RezervasyonTarihi = DateTime.Now,
                ToplamFiyat = 1000,
                KullaniciId = 1
            };
            TimeSpan timeSpan = rezervasyon.CikisTarihi - rezervasyon.GirisTarihi;
           var otel= db.Rezervasyon.Add(rezervasyon);
            db.SaveChanges();

            
           //bir kez çalışan ve belli bir zaman dolduktan sonra çalışır.
            var otelRandevu = BackgroundJob.Schedule(() => OtelRezervasyonDurumuDegistir(otel.Entity.OdaId), timeSpan);

            return Ok();
        }


        [HttpGet]
        [Route("faturagonder/{userId}")]
        public ActionResult FaturaMailGonder(int userId)
        {
            userId = 1;
            //haftalık mail gönderme işlemi
            //her hafta işlem gerçekleşir.
            RecurringJob.AddOrUpdate(()=>MailGonder(userId),Cron.Weekly);
          
            return Ok();
        }

        [HttpGet]
       
        public ActionResult OdaRezervasyonVeMailGonder(int userId)
        {
            //Rezervasyon bittikten sonra odanın durumu değişir ve mail gönderilir.
            Rezervasyon rezervasyon = new Rezervasyon
            {
                CikisTarihi = DateTime.Now.AddDays(3),
                GirisTarihi = DateTime.Now.AddDays(1),
                Fiyat = 500,
                OdaId = 5,
                RezervasyonTarihi = DateTime.Now,
                ToplamFiyat = 1000,
                KullaniciId = 1
            };
            TimeSpan timeSpan = rezervasyon.CikisTarihi - rezervasyon.GirisTarihi;
            var otel = db.Rezervasyon.Add(rezervasyon);
            db.SaveChanges();

            var otelRandevu = BackgroundJob.Schedule(() => OtelRezervasyonDurumuDegistir(otel.Entity.OdaId), timeSpan);

            BackgroundJob.ContinueJobWith(otelRandevu, () => MailGonder(rezervasyon.KullaniciId));
            return Ok();
        }

        public void MailGonder(int userId)
        {
            var user = db.Kullanici.Find(userId);
            string mail = user.Mail;
            //Mail Gönderme İşlemleri
        }

        public void OtelRezervasyonDurumuDegistir(int odaId)
        {
            var oda = db.Oda.Find(odaId);
            oda.OdaDurumu = true;
            db.SaveChanges();

        }

        public void ProcessFireAndForgetJob()
        {
            //Job create edildikten sonra çalışır ve process olur.

            db.Kullanici.Add(new Kullanici
            {
                KullaniciAdi = "Hangfireileolustu",
                RoleId = 1


            });
            db.SaveChanges();

        }

      
    }
}
