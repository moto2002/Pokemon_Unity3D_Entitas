﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace PokemonBattelePokemon
{

    [Serializable]
    public class Pokemon:ScriptableObject
    {

        public void InitPokemon(int raceID)
        {
            
            if (raceID < 1 || raceID > ResourceController.Instance.allRaceDic.Count) return;
            this.raceID = raceID;
            skillList = new List<int>();
            isMan = UnityEngine.Random.Range(-1, 1) >= 0;
            basestats = new Basestats();
            iv = new IndividualValues();
            var nature = PokeNature;
            var name = Ename;

        }

        private void UpdataRace()
        {
            if (raceID <= 0 || raceID > ResourceController.Instance.allRaceDic.Count)
            {
                Debug.LogError("RaceID越界");
                //race = ResourceController.Instance.allRaceDic[1];
            }
            else race = ResourceController.Instance.allRaceDic[raceID];
        }
        #region 属性
        //基础属性

        //public int currentLife;//实际生命

        ///// <summary>
        ///// 当前生命
        ///// </summary>
        //public int CurrentLife
        //{
        //    get
        //    {
        //        return currentLife;
        //    }
        //    set
        //    {
        //        currentLife = value;
        //    }
        //}

        ///// <summary>
        ///// Hp能力值
        ///// </summary>
        //public int FullLife
        //{
        //    get
        //    {
        //        //HP能力值 ＝ （种族值×2＋基础点数÷4＋个体值）×等级÷100＋等级＋10
        //        return CalBase(PokeRace.health, Basestats.Health, IV.Health) + 5;

        //    }
        //}

        //非能力值 ＝ （（种族值×2＋基础点数÷4＋个体值）×等级÷100＋5）×性格修正
        ///// <summary>
        ///// 计算能力值
        ///// 返回（种族值×2＋基础点数÷4＋个体值）×等级÷100＋5）
        ///// </summary>
        ///// <param name="raceVal"></param>
        ///// <param name="BaseStatVal"></param>
        ///// <param name="IVVal"></param>
        ///// <returns></returns>
        //private int CalBase(int raceVal, int BaseStatVal, int IVVal)
        //{
        //    float f = (float)level / 100;
        //    return (int)((raceVal * 2 + BaseStatVal / 4 + IVVal) * f + level + 5);
        //}

        ///// <summary>
        ///// 物攻能力值
        ///// </summary>
        //public int PhysicPower
        //{
        //    get
        //    {
        //        return (int)(CalBase(PokeRace.phyPower, Basestats.PhysicPower, IV.PhysicPower) * (PokeNature.PhysicPowerAffect + 1));
        //    }
        //}
        ///// <summary>
        ///// 物防能力值
        ///// </summary>
        //public int PhysicDefence
        //{
        //    get
        //    {
        //        return (int)(CalBase(PokeRace.phyDefence, Basestats.PhysicDefence, IV.PhysicDefence) * (1 + PokeNature.PhysicDefenceAffect));
        //    }
        //}

        ///// <summary>
        ///// 特攻能力值
        ///// </summary>
        //public int EnergyPower
        //{
        //    get
        //    {
        //        return (int)(CalBase(PokeRace.energyPower, Basestats.EnergyPower, IV.EnergyPower) * (1 + PokeNature.EnergyPowerEffect));
        //    }
        //}

        ///// <summary>
        ///// 特防能力值
        ///// </summary>
        //public int EnergyDefence
        //{
        //    get
        //    {
        //        return (int)(CalBase(PokeRace.energyDefence, Basestats.EnergyDefence, IV.EnergyDefence) * (1 + PokeNature.EnergyDefenceEffect));
        //    }
        //}

        ///// <summary>
        ///// 速度能力值
        ///// </summary>
        //public int Speed
        //{
        //    get
        //    {
        //        return (int)(CalBase(PokeRace.speed, Basestats.Speed, IV.Speed) * (1 + PokeNature.SpeedAffect));
        //    }
        //}
        //等级
        private const int level = 100;
        public int Level
        {
            get
            {
                return level;
            }
        }

        /// <summary>
        /// 昵称
        /// </summary>
        [SerializeField]
        public string ename;
        public string Ename
        {
            get
            {
                if (ename == ""||ename == null) ename = Name;
                return ename;
            }

            set
            {
                ename = value;
            }
        }
        //种族名称
        public string Name
        {
            get
            {
                if (null != PokeRace ) return PokeRace.sname;
                return "???";
            }
        }

        [SerializeField]
        public Nature nature;
        public Nature PokeNature
        {
            get
            {
                if (null == nature )
                {
                    //性格随机获取
                    int NatureCount = ResourceController.Instance.allNatureDic.Count;
                    int ran = UnityEngine.Random.Range(0, NatureCount);
                    Array arr = Enum.GetValues(typeof(NatureType));
                    NatureType _natureType = (NatureType)arr.GetValue(ran);
                    nature = ResourceController.Instance.allNatureDic[_natureType];
                }
                return nature;
            }
            set
            {
                nature = value;
            }


        }

        public String NatureName
        {
            get
            {
                return PokeNature.NatureName;
            }
        }
        //种族
        public int raceID = 1;
        private Race race;

        /// <summary>
        /// 种族
        /// </summary>
        public Race PokeRace
        {
            get
            {
                if (null == race)
                {
                    UpdataRace();
                }
                return race;
            }
            set
            {
                race = value;
            }
        }
        [SerializeField]
        private AbilityType abilitytype;
        /// <summary>
        /// 外在表现特性
        /// </summary>
        public AbilityType ShowAbility
        {
            get
            {
                if (abilitytype == 0)
                {

                    abilitytype = PokeRace.firstAbility;
                    if (PokeRace.secondAbility != 0 && PokeRace.hideAbility != 0)
                    {
                        //有三个特性
                        float r = UnityEngine.Random.Range(0, 2.5f);
                        if (r >= 2) abilitytype = PokeRace.hideAbility;
                        else if (r >= 1) abilitytype = PokeRace.secondAbility;
                    }
                    else if (PokeRace.secondAbility != 0)
                    {
                        //没有隐藏特性
                        float r = UnityEngine.Random.Range(0, 2);
                        if (r >= 1) abilitytype = PokeRace.secondAbility;
                    }
                    else if (PokeRace.hideAbility != 0)
                    {
                        //没有第二特性
                        float r = UnityEngine.Random.Range(0, 1.5f);
                        if (r >= 1) abilitytype = PokeRace.hideAbility;
                    }

                }
                return abilitytype;
            }
            set
            {
                abilitytype = value;
            }
        }
        public AbilityType HideAbility
        {
            get
            {
                return PokeRace.hideAbility;
            }
            set
            {
                PokeRace.hideAbility = value;
            }
        }
        //可使用技能
        [SerializeField]
        public List<int> skillList;
        //public List<Skill> skillList = new List<Skill>();
        //public int FindSkillIndex(Skill skill)
        //{
        //    for (int i = 0; i < 4 && i < skillList.Count; i++)
        //    {
        //        if (skillList[i].SKillID == skill.SKillID)
        //        {
        //            return i;
        //        }
        //    }
        //    return -1;
        //}
        [SerializeField]
        public bool isMan ;
        //努力值
        [SerializeField]
        private Basestats basestats ;
        /// <summary>
        /// 努力值
        /// </summary>
        public Basestats Basestats
        {
            get
            {
                return basestats;
            }
            set
            {
                basestats = value;
            }

        }
        //个体值
        [SerializeField]
        private IndividualValues iv;
        /// <summary>
        /// 个体值
        /// </summary>
        public IndividualValues IV
        {
            get
            {
                return iv ?? new IndividualValues();
            }
            set
            {
                iv = value;
            }
        }

        #endregion


    }
}