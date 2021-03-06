﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MonitorizareVot.Ong.Api.Extensions;
using MonitorizareVot.Ong.Api.ViewModels;
using MonitorizareVot.Ong.Api.Filters;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;

namespace MonitorizareVot.Ong.Api.Controllers
{
    [Route("api/v1/raspunsuri")]
    //[ValidateModelState]
    public class Raspunsuri : Controller
    {
        private readonly IMediator _mediator;
        private readonly IConfigurationRoot _configuration;

        public Raspunsuri(IMediator mediator, IConfigurationRoot configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        /// <summary>
        /// Returneaza lista sectiilor de votare unde au raspuns observatorii care apartion ONG-ului cu id-ul primit
        /// la intrebarile din formulare marcate cu RaspunsFlag == Urgent ordonate desc dupa DataModificare
        /// </summary>
        /// <param name="model"> Detaliile de paginare (default Page=1, PageSize=5)
        /// Urgent (valoarea campului RaspunsFlag)
        /// </param>
        [HttpGet]
        public async Task<ApiListResponse<RaspunsModel>> Get(FiltruRaspunsuriModel model)
        {
            // Mock data
            //return await Task.FromResult(
            //    new ApiListResponse<RaspunsModel>
            //    {
            //        Data = new List<RaspunsModel>
            //        {
            //             new RaspunsModel {Observator = "Ionescu Vasile", Sectie = "BU 123", IdSectie = 2},
            //             new RaspunsModel {Observator = "Popescu Ionut", Sectie = "BU 123", IdSectie = 2},
            //             new RaspunsModel {Observator = "Ionescu Maria", Sectie = "CT 13", IdSectie = 76},
            //             new RaspunsModel {Observator = "ALbertin Merisor", Sectie = "IS 13", IdSectie = 67},
            //             new RaspunsModel {Observator = "Vasilian Cristi", Sectie = "IS 123", IdSectie = 66},
            //             new RaspunsModel {Observator = "Zorii Maria", Sectie = "CT 143", IdSectie = 78},
            //             new RaspunsModel {Observator = "Gheorghe Marian", Sectie = "CT 6", IdSectie = 77},
            //             new RaspunsModel {Observator = "Ionescu Vasile", Sectie = "BU 124", IdSectie = 88},
            //             new RaspunsModel {Observator = "Cernica Maria", Sectie = "GR 99", IdSectie = 98},
            //             new RaspunsModel {Observator = "Vlasceanu Ionut", Sectie = "TM 33", IdSectie = 99},
            //        },
            //        Page = 1,
            //        PageSize = 10,
            //        TotalItems = 300
            //    });

            var organizator = this.GetOrganizatorOrDefault(_configuration.GetValue<bool>("DefaultOrganizator"));
            var idOng = this.GetIdOngOrDefault(_configuration.GetValue<int>("DefaultIdOng"));

            return await _mediator.SendAsync(new RaspunsuriQuery
            {
                IdONG = idOng,
                Organizator = organizator,
                Page = model.Page,
                PageSize = model.PageSize,
                Urgent = model.Urgent
            });
        }

        /// <summary>
        /// Returneaza raspunsurile date de un observator pentru o anumita sectie de votare
        /// </summary>
        /// <param name="IdSectieDeVotare">Id-ul sectiei unde s-au completat raspunsurile</param>
        /// <param name="IdObservator">Id-ul observatorului care a dat raspunsurile</param>
        [HttpGet("RaspunsuriCompletate")]
        public async Task<List<IntrebareModel<RaspunsCompletatModel>>> Get(FiltruRaspunsuriCompletateModel model)
        {
            return await _mediator.SendAsync(new RaspunsuriCompletateQuery
            {
                IdObservator = model.IdObservator,
                IdSectieDeVotare = model.IdSectieDeVotare
            });
        }
    }
}