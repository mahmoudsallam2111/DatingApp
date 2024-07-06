using AutoMapper;
using DatingApp.Application.Helpers;

namespace DatingApp.Application.AutoMapper
{
    public class PagesListConverter<TSource, TDestination> : ITypeConverter<PagesList<TSource>, PagesList<TDestination>>
    {
        public PagesList<TDestination> Convert(PagesList<TSource> source, PagesList<TDestination> destination, ResolutionContext context)
        {
            var mappedItems = context.Mapper.Map<List<TDestination>>(source.ToList());
            return new PagesList<TDestination>(mappedItems, source.TotalCount, source.CurrentPage, source.PageSize);
        }
    }

}
