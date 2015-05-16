using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using KladrTask.Domain.Abstract;
using PrefixTree;

namespace KladrTask.WebUI
{
    public class KladrRepositoryHelper
    {
        private readonly IKladrRepository repository;
        private readonly Tree tree;
        public KladrRepositoryHelper(IKladrRepository repository)
        {
            this.repository = repository;
            tree = new Tree();
            FillRegionsTree();
        }

        public void FillRegionsTree()
        {
            foreach (var region in repository.Regions.OrderBy(region => region.Code))
            {
                tree.Insert(region.Code, region.Name);
            }
        }

        public List<SelectListItem> GetRegions(string selectedRegion = "")
        {
            return repository.Regions
                             .ToList()
                             .Where(r => r.Level == 1)
                             .Select(region => new SelectListItem()
                             {
                                 Text = region.Name, Value = region.Code, Selected = region.Code == selectedRegion
                             })
                             .ToList();
        }

        public List<SelectListItem> GetLocalities(string regionCode, string selectedLocality = "")
        {
            var states = new List<SelectListItem>() { new SelectListItem() { Text = "Выберете регион", Value = "" } };
            states.AddRange(tree.GetChilds(regionCode).Select(child => new SelectListItem()
            {
                Text = child.Name, Value = child.Code, Selected = child.Code == selectedLocality
            }));

            return states;
        }

        public List<SelectListItem> GetStreets(string localityCode, string selectedStreet = "")
        {
            var streets = new List<SelectListItem>() { new SelectListItem() { Text = "Выберете улицу", Value = "" } };
            var code = localityCode.Substring(0, 11);
            foreach (var street in repository.Roads.Where(st => st.Code.StartsWith(code)))
            {
                streets.Add(new SelectListItem()
                {
                    Text = street.Name, Value = street.Code, Selected = street.Code == selectedStreet
                });
            }

            return streets;
        }

        public List<SelectListItem> GetIndexes(string regionCode, string localityCode, string roadCode)
        {
            if (!string.IsNullOrEmpty(roadCode))
            {
                var indexes = GetIndexFromRoad(roadCode).ToList();
                if (indexes.Count > 0)
                    return indexes.Select(r => new SelectListItem()
                    {
                        Text = r.Item1, Value = r.Item2
                    }).ToList();
            }

            if (!string.IsNullOrEmpty(localityCode))
            {
                var indexes = GetIndexFromLocality(localityCode).ToList();
                if (indexes.Count > 0)
                    return indexes.Select(l => new SelectListItem()
                    {
                        Text = l.Item1, Value = l.Item2
                    }).ToList();
            }

            return GetIndexFromRegion(regionCode).Select(r => new SelectListItem()
            {
                Text = r.Item1, Value = r.Item2
            }).ToList();
        }

        private IEnumerable<Tuple<string, string>> GetIndexFromRegion(string regionCode)
        {
            var region = repository.Regions.FirstOrDefault(r => r.Code == regionCode);
            if (region != null && !string.IsNullOrEmpty(region.Index))
                yield return Tuple.Create(region.Index, region.Index);
        }

        private IEnumerable<Tuple<string, string>> GetIndexFromLocality(string localityCode)
        {
            var locality = repository.Regions.FirstOrDefault(r => r.Code == localityCode);
            if (locality != null && !string.IsNullOrEmpty(locality.Index))
                yield return Tuple.Create(locality.Index, locality.Index);
        }

        private IEnumerable<Tuple<string, string>> GetIndexFromRoad(string roadCode)
        {
            var road = repository.Roads.FirstOrDefault(r => r.Code == roadCode);

            if (road != null && !string.IsNullOrEmpty(road.Index))
            {
                yield return Tuple.Create(road.Index, road.Index);
                yield break;
            }

            var code = roadCode.Substring(0, 15);
            foreach (var house in repository.Houses.Where(h => h.Code.StartsWith(code)))
            {
                yield return Tuple.Create(house.Index + " " + house.Name, house.Index);
            }
        }
    }
}