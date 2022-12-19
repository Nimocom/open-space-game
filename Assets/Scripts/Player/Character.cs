using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Character : MonoBehaviour
{
    public enum Fraction
    {
        Player,
        Military,
        Pirate,
        Civilian,
        HeadHunter,
        Trader
    }
        
    public Hull hull;

    public Character.Fraction fraction;
    public string characterID;
    public new string name;
    public int avatarID;
    public int credits;
    public int level;

    float[][] relation =
        {
            //                   X   M   P   C   H   T
            /*X*/  new float[] { 1                    },
            /*M*/  new float[] { 1,  1                },
            /*P*/  new float[] {-1, -1,  1            },
            /*C*/  new float[] { 0,  1, -1,  1        },
            /*H*/  new float[] { 0,  0,  0,  0,  1    },
            /*T*/  new float[] { 0,  0, -1,  0,  0,  1},
        };

    public float GetRelation(Fraction otherFraction)
    {
        int i = (int)fraction;
        int j = (int)otherFraction;
        return (i > j) ? relation[i][j] : relation[j][i];
    }

    public void SetRelation(Fraction otherFraction, float val)
    {
        int i = (int)fraction;
        int j = (int)otherFraction;
        if (i > j)
            relation[i][j] -= val;
        else
            relation[j][i] -= val;
    }

    public CharacterData GetCharacterData()
    {
        return new CharacterData()
        {
            avatarID = this.avatarID,
            characterID = this.characterID,
            fraction = this.fraction,   
            credits = this.credits,
            level = this.level,
            name = this.name
        };
    }

    public void SetCharacterData(CharacterData data)
    {
        avatarID = data.avatarID;
        characterID = data.characterID;
        fraction = data.fraction;
        credits = data.credits;
        level = data.level;
        name = data.name;
    }
}
    

