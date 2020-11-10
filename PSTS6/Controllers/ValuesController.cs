using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JsonHelper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PSTS6.HelperClasses;
using PSTS6.Repository;


namespace PSTS6.Controllers
{
    [Route("api")]
    [ApiController]
    public class ValuesController : ControllerBase
    {

        private readonly IRepository _repo;
        
        public ValuesController(IRepository repo)
        {
            _repo = repo;
            
        }

        [HttpGet("{id}")]
        public string Get(string id)


        {

           

            IGoogleVisualizable listMapper = new ListMapper(_repo);

            List<IGoogleVisualizable> mappedList = listMapper.ConvertLists(Convert.ToInt32(id));

            string value = JSONHelper.BuildArray(mappedList);

            return value;

        }

    }
}
