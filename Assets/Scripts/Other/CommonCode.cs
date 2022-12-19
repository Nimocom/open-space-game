using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable
{
    void TakeDamage(float hullDamage, float shieldDamage, Character character);
}

public interface IRemovable
{
    void RemoveFromScene();
}

public static class Helper
{
    public static string ToStringWH(this Resolution res)
    {
        return res.width + " X " + res.height;    
    }
}

public class ModuleComparer<T> : IComparer<T> where T : Module
{
    public int Compare(T x, T y)
    {
        if (x == null || y == null)
            return Comparer<object>.Default.Compare(y, x);
        else
        {
            if (x.moduleType > y.moduleType)
                return 1;
            else if (x.moduleType < y.moduleType)
                return -1;
            else
                return 0;
        }
    }
}
