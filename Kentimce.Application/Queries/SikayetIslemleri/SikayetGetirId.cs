using AutoMapper;
using Kentimce.Application.Abstractions;
using Kentimce.Application.Responses;
using MediatR;


namespace Kentimce.Application.Queries.SikayetIslemleri
{
    public class SikayetGetirId : IRequest<BaseResponse<SikayetResponse>>
    {
        public string id { get; set; }
    }

    public class DefterTurQueryHandler : IRequestHandler<SikayetGetirId, BaseResponse<SikayetResponse>>
    {

        private readonly IAppDbContext _context;
        private readonly IMapper _mapper;
      


        public DefterTurQueryHandler(IAppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<BaseResponse<SikayetResponse>> Handle(SikayetGetirId request, CancellationToken cancellationToken)
        {

            var response = new BaseResponse<SikayetResponse>();

            try
            {


                var s = await _context.Sikayetler.FindAsync(request.id);
                if (s == null) return response.NoContent();

                var result = _mapper.Map<SikayetResponse>(s);
                return response.Success(result);

            }
            catch (Exception ex)
            {

               return response.Fail("Servis Exception: " + ex.Message);
               
            }
        }



    }
}