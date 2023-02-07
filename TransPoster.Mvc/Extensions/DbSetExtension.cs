using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace TransPoster.Mvc.Extensions;

public static class ContextSetExtension
{ 
    // public static IQueryable Set(this DbContext _context, Type t)
    // {
    //
    //     Type dbSetType = typeof(DbSet<>);
    //     var dbSetGenericType = dbSetType.MakeGenericType(t);
    //     dbSetGenericType.InvokeMember("Set");
    //     
    //     var res= _context.GetType().GetMethod("Set").MakeGenericMethod(t).Invoke(_context, null);
    //     return (IQueryable)res;
    // }


    public static IQueryable<T>Set2<T>(this DbContext _context, T t) 
    {
        var typo = t.GetType();
        return (IQueryable<T>)_context.GetType().GetMethod("Set").MakeGenericMethod(typo).Invoke(_context, null);
    }
}