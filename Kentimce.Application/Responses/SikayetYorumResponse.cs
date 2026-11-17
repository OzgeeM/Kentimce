using AutoMapper;
using Kentimce.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kentimce.Application.Responses
{
    public class SikayetYorumResponse
    {
        public Guid Id { get; set; }
        public Guid SikayetId { get; set; }
        public Guid KullaniciId { get; set; }
        public string Yorum { get; set; }
        public string kullaniciAdi { get; set; }
        
    }

    public class SikayetYorumResponseProfile : Profile
    {
        public SikayetYorumResponseProfile()
        {
            CreateMap<SikayetYorum, SikayetYorumResponse>()
                 .IgnoreAllPropertiesWithAnInaccessibleSetter()
                 .ForMember(x => x.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.kullaniciAdi, opt => opt.MapFrom(src => src.kullanici.Ad));//tc ile değiş yada ekle

        }
    }
}
