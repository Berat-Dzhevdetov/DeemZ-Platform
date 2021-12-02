namespace DeemZ.Services.CertificateServices
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;
    using System.Linq;
    using DeemZ.Data;

    public class CertificateService : ICertificateService
    {
        private readonly DeemZDbContext context;
        private readonly IMapper mapper;

        public CertificateService(DeemZDbContext context, IMapper mapper)
        {
            this.context = context;
            this.mapper = mapper;
        }

        public T GetCertificateByExternalNumber<T>(int externalNumber)
            => context.Certificates
                .Where(x => x.ExternalNumber == externalNumber)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .FirstOrDefault();

        public T GetCertificateById<T>(string cid)
            =>  context.Certificates
                .Where(x => x.Id == cid)
                .ProjectTo<T>(mapper.ConfigurationProvider)
                .FirstOrDefault();
        

    }
}