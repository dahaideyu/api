using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Koowoo.Domain;

namespace Koowoo.Services
{
    public static class AreaExtensions
    {
        /// <summary>
        /// Sort categories for tree representation
        /// </summary>
        /// <param name="source">Source</param>
        /// <param name="parentId">Parent category identifier</param>
        /// <param name="ignoreCategoriesWithoutExistingParent">A value indicating whether categories without parent category in provided category list (source) should be ignored</param>
        /// <returns>Sorted categories</returns>
        public static IList<AreaEntity> SortElementsForTree(this IList<AreaEntity> source, string parentCode = "", bool ignoreCategoriesWithoutExistingParent = false)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var result = new List<AreaEntity>();

            foreach (var cat in source.Where(c => c.ParentCode == parentCode).ToList())
            {
                result.Add(cat);
                result.AddRange(SortElementsForTree(source, cat.Code, true));
            }

            if (!ignoreCategoriesWithoutExistingParent && result.Count != source.Count)
            {
                //find categories without parent in provided category source and insert them into result
                foreach (var cat in source)
                    if (result.FirstOrDefault(x => x.AreaUUID == cat.AreaUUID) == null)
                        result.Add(cat);
            }
            return result;
        }
    }
}
