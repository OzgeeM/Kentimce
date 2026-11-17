using AutoMapper;
using Kentimce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.Responses
{
    public class SikayetResponse
    {
        public Guid Id { get; set; }

        public Guid? KullaniciId { get; set; }
        public string KullaniciAdi { get; set; }
        public string KullaniciSoyad { get; set; }
        public string KullaniciTc { get; set; }
        public Guid BelediyeId { get; set; }

        public string BelediyeAdi { get; set; }
        public string Baslik { get; set; }
        public string Aciklama { get; set; }
        public string Adres { get; set; }
        public string Durum { get; set; }
        public bool OnInceleme { get; set; }


        public List<SikayetEk> ekler { get; set; }
        public List<SikayetDurumResponse> durumGecmisleri { get; set; }
        public List<SikayetYorum> yorumlar { get; set; }
        public List<SikayetBegeni> begeniler { get; set; }
    }

    public class SikayetResponseProfile : Profile
    {
        public SikayetResponseProfile()
        {
            CreateMap<Sikayet, SikayetResponse>()
                 .IgnoreAllPropertiesWithAnInaccessibleSetter()
                 .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.BelediyeAdi, opt => opt.MapFrom(src => src.belediye.Isim))
            .ForMember(dest => dest.KullaniciAdi, opt => opt.MapFrom(src => src.kullanici.Ad))
            .ForMember(dest => dest.KullaniciSoyad, opt => opt.MapFrom(src => src.kullanici.Soyad));
            //.ForMember(dest => dest.KullaniciTc, opt => opt.MapFrom(src => src.kullanici.Tc))//EKLE!

            CreateMap<SikayetEk, SikayetEkResponse>()
           .IgnoreAllPropertiesWithAnInaccessibleSetter()
           .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
            .ForMember(x => x.Id, opt => opt.MapFrom(src => src.sikayet.Id));

            CreateMap<SikayetDurum, SikayetDurumResponse>()
                 .IgnoreAllPropertiesWithAnInaccessibleSetter()
                 .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                  .ForMember(x => x.Id, opt => opt.MapFrom(src => src.sikayet.Id));

            CreateMap<SikayetYorum, SikayetYorumResponse>()
               .IgnoreAllPropertiesWithAnInaccessibleSetter()
               .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
                .ForMember(x => x.Id, opt => opt.MapFrom(src => src.sikayet.Id));

            CreateMap<SikayetBegeni, SikayetBegeniResponse>()
              .IgnoreAllPropertiesWithAnInaccessibleSetter()
              .IgnoreAllSourcePropertiesWithAnInaccessibleSetter()
               .ForMember(x => x.Id, opt => opt.MapFrom(src => src.sikayet.Id));


        }
    }
}
