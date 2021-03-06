﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MonitorizareVot.Domain.Ong.Models;
using MonitorizareVot.Ong.Api.Extensions;
using MonitorizareVot.Ong.Api.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MonitorizareVot.Ong.Api.Queries
{
    public class RaspunsuriQueryHandler :
        IAsyncRequestHandler<RaspunsuriQuery, ApiListResponse<RaspunsModel>>,
        IAsyncRequestHandler<RaspunsuriCompletateQuery, List<IntrebareModel<RaspunsCompletatModel>>>
    {
        private readonly OngContext _context;
        private readonly IMapper _mapper;

        public RaspunsuriQueryHandler(OngContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ApiListResponse<RaspunsModel>> Handle(RaspunsuriQuery message)
        {
            IQueryable<SectieModel> sectiiCuObservatori = _context.Raspuns
                .Where(r => message.Organizator || r.IdObservatorNavigation.IdOng == message.IdONG)
                .Where(r => r.IdRaspunsDisponibilNavigation.RaspunsCuFlag == message.Urgent)
                .Select(y => new SectieModel
                {
                    IdObservator = y.IdObservator,
                    Observator = y.IdObservatorNavigation.NumeIntreg,
                    IdSectie = y.IdSectieDeVotare,
                    NumarSectie = y.IdSectieDeVotareNavigation.NumarSectie,
                    CodJudet = y.IdSectieDeVotareNavigation.IdJudetNavigation.CodJudet,
                    Sectie = y.IdSectieDeVotareNavigation.DenumireUat,
                    DataUltimeiModificari = y.DataUltimeiModificari
                });

            var sectiiCuObservatoriPaginat = await sectiiCuObservatori
                 .OrderByDescending(s => s.DataUltimeiModificari)
                 .Distinct()
                 .Skip((message.Page - 1) * message.PageSize)
                 .Take(message.PageSize)
                 .ToListAsync();

            return new ApiListResponse<RaspunsModel>
            {
                Data = sectiiCuObservatoriPaginat.Select(x => _mapper.Map<RaspunsModel>(x)).ToList(),
                Page = message.Page,
                PageSize = message.PageSize,
                TotalItems = await sectiiCuObservatori.CountAsync()
            };
        }

        public async Task<List<IntrebareModel<RaspunsCompletatModel>>> Handle(RaspunsuriCompletateQuery message)
        {
            var raspunsuri = await _context.Raspuns
                .Include(r => r.IdRaspunsDisponibilNavigation)
                    .ThenInclude(rd => rd.IdIntrebareNavigation)
                .Include(r => r.IdRaspunsDisponibilNavigation)
                    .ThenInclude(rd => rd.IdOptiuneNavigation)
                .Where(r => r.IdObservator == message.IdObservator && r.IdSectieDeVotare == message.IdSectieDeVotare)
                .ToListAsync();

            var intrebari = raspunsuri
                .Select(r => r.IdRaspunsDisponibilNavigation.IdIntrebareNavigation)
                .ToList();

            return intrebari.Select(i => _mapper.Map<IntrebareModel<RaspunsCompletatModel>>(i)).ToList();
        }
    }
}
