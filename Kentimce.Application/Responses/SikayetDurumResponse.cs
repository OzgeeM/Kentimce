using AutoMapper;
using Kentimce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.Responses
{
    public class SikayetDurumResponse
    {
        public Guid Id { get; set; }
        public string EskiDurum { get; set; }
        public string YeniDurum { get; set; }
        public string Aciklama { get; set; }
        public Guid DegistirenKullaniciId { get; set; }
        public string DegistirenKullaniciAdi { get; set; }
        public DateTime Tarih { get; set; }

    }

    public class SikayetDurumResponseProfile : Profile
    {
        public SikayetDurumResponseProfile()
        {
            CreateMap<SikayetDurum, SikayetDurumResponse>()
                 .IgnoreAllPropertiesWithAnInaccessibleSetter()
                 .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.DegistirenKullaniciAdi, opt => opt.MapFrom(src => src.IslemYapanKullanici.Ad)); //tc ile değiş!
            
        }
    }
}
