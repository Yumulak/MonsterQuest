using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class CharacterEvents
{
    //static events invoked by Damagable components using Damagable script call their respective method in UIManager for damage or health

    //character damaged and damage value
    public static UnityAction<GameObject, int> characterDamaged;
    //character healed and amount healed
    public static UnityAction<GameObject, int> characterHealed;
}

