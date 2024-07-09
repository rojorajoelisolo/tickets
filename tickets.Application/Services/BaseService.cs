using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace tickets.Application.Services
{
    public class BaseService
    {
        protected IMapper _mapper;

        public BaseService(IMapper mapper)
        {
            _mapper = mapper;
        }
    }
}
