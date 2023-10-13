using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace _20230907_ConsoleProject
{
    public enum JobEnum
    {
        JOB1 = 1,
        JOB2
    }

    public enum SkillEnum
    {
        ATTACK,
        HEAL,
        SKILL,
        ESCAPE,
    }

    public enum MonsterEnum
    {
        MONSTER1,
        MONSTER2,
        MONSTER3,
    }

    class Object
    {
        public int hp; // 체력
        public int attack; // 공격력
        public int exp; // 경험치
        public int money; // 재화
    }

    class Player : Object
    {
        public string name;
        public int level;
        public Weapon weapon;
        public Job job;
        public Skill[] skill = new Skill[4]; // skill

        public Player ()
        {
            level = 1;
            exp = 0;
            money = 0;
        }

        public void PlayerJobSetting(JobEnum jobEnum)
        { // 직업을 선택했을 때 직업에 따른 플레이어 셋팅
            hp = 0;
            switch (jobEnum)
            {
                case JobEnum.JOB1:
                    job = new Job("Job1", 50, 30, JobEnum.JOB1);
                    hp += job.plusHp;
                    attack += job.plusAttack;
                    weapon = new Weapon("Job1 무기", 30);
                    // 스킬 세팅
                    skill[(int)SkillEnum.ATTACK] = new Skill("공격", attack + weapon.plusDamage, 0);
                    skill[(int)SkillEnum.HEAL] = new Skill("회복", attack / 2 + weapon.plusDamage, 1);
                    skill[(int)SkillEnum.SKILL] = new Skill("강화공격", attack + weapon.plusDamage * 3, 2);
                    skill[(int)SkillEnum.ESCAPE] = new Skill("도망치기", 0, 0);
                    break;
                case JobEnum.JOB2:
                    job = new Job("Job2", 40, 20, JobEnum.JOB2);
                    hp += job.plusHp;
                    attack += job.plusAttack;
                    weapon = new Weapon("Job2 무기", 20);
                    // 기본 세팅
                    skill[(int)SkillEnum.ATTACK] = new Skill("공격", attack + weapon.plusDamage, 0);
                    skill[(int)SkillEnum.HEAL] = new Skill("회복", attack / 2 + weapon.plusDamage * 2, 1);
                    skill[(int)SkillEnum.SKILL] = new Skill("강화공격", attack + weapon.plusDamage * 2, 2);
                    skill[(int)SkillEnum.ESCAPE] = new Skill("도망치기", 0, 0);
                    break;
            }
        }

        public void PlayerLevelUp (Skill[] skill)
        {
            int currentLevel = level;
            if (this.level * 1000 <= this.exp)
            {
                this.hp += 1000;
                if (hp >= 5000)
                {
                    hp = 5000;
                }
                this.attack += 100;
                if (job.jobEnum == JobEnum.JOB1)
                {
                    skill[(int)SkillEnum.ATTACK].attck = attack + weapon.plusDamage;
                    skill[(int)SkillEnum.HEAL].attck = attack / 2 + weapon.plusDamage;
                    skill[(int)SkillEnum.SKILL].attck = attack + weapon.plusDamage * 3;
                }
                else
                {
                    skill[(int)SkillEnum.ATTACK].attck = attack + weapon.plusDamage;
                    skill[(int)SkillEnum.HEAL].attck = attack / 2 + weapon.plusDamage * 2;
                    skill[(int)SkillEnum.SKILL].attck = attack + weapon.plusDamage * 2;
                }
                this.level++;
                this.exp = 0;
                Console.WriteLine("레벨 업!");
                Console.WriteLine($"Lv. {currentLevel} -> Lv. {level}");
                Thread.Sleep(1500);
                Console.Clear();
            }
        }
    }

    class Monster : Object
    {
        public string name;
        public bool firstAttack; // 선공이면 true
        public Map map;

        public void DeadMonster(Player player)
        {
            player.exp += this.exp;
            player.money += this.money;
            Console.WriteLine($"{this.name}을 해치웠다!\n{player.name}은 {this.exp}만큼의 경험치와 {this.money}만큼의 돈을 얻었다.\n");
        }


        public void MonsterSetting(Monster[] monsters)
        {
            for (int i = 0; i < monsters.Length; i++)
            {
                monsters[i] = new Monster();
            }

            // 이름, 체력, 공격력, 경험치, 돈, 선공
            monsters[0].name = "Moster1";
            monsters[0].hp = 50;
            monsters[0].attack = 5;
            monsters[0].exp = 100;
            monsters[0].money = 1000;
            monsters[0].map = Map.MAP1;
            monsters[0].firstAttack = false;

            monsters[1].name = "Moster2";
            monsters[1].hp = 100;
            monsters[1].attack = 10;
            monsters[1].exp = 150;
            monsters[1].money = 2000;
            monsters[1].map = Map.MAP1;
            monsters[1].firstAttack = false;

            monsters[2].name = "Moster3";
            monsters[2].hp = 150;
            monsters[2].attack = 15;
            monsters[2].exp = 300;
            monsters[2].money = 3000;
            monsters[2].map = Map.MAP1;
            monsters[2].firstAttack = true;

            monsters[3].name = "Moster4";
            monsters[3].hp = 250;
            monsters[3].attack = 20;
            monsters[3].exp = 400;
            monsters[3].money = 10000;
            monsters[3].map = Map.MAP2;
            monsters[3].firstAttack = false;

            monsters[4].name = "Moster5";
            monsters[4].hp = 350;
            monsters[4].attack = 60;
            monsters[4].exp = 700;
            monsters[4].money = 15000;
            monsters[4].map = Map.MAP2;
            monsters[4].firstAttack = true;

            monsters[5].name = "Moster6";
            monsters[5].hp = 500;
            monsters[5].attack = 40;
            monsters[5].exp = 1000;
            monsters[5].money = 25000;
            monsters[5].map = Map.MAP2;
            monsters[5].firstAttack = true;

            monsters[6].name = "Moster7";
            monsters[6].hp = 700;
            monsters[6].attack = 100;
            monsters[6].exp = 3000;
            monsters[6].money = 40000;
            monsters[6].map = Map.MAP3;
            monsters[6].firstAttack = false;

            monsters[7].name = "Moster8";
            monsters[7].hp = 1000;
            monsters[7].attack = 150;
            monsters[7].exp = 3000;
            monsters[7].money = 60000;
            monsters[7].map = Map.MAP3;
            monsters[7].firstAttack = true;

            monsters[8].name = "Moster9";
            monsters[8].hp = 1500;
            monsters[8].attack = 200;
            monsters[8].exp = 6000;
            monsters[8].money = 100000;
            monsters[8].map = Map.MAP3;
            monsters[8].firstAttack = true;

            monsters[9].name = "Boss";
            monsters[9].hp = 10000;
            monsters[9].attack = 300;
            monsters[9].exp = 10000;
            monsters[9].money = 200000;
            monsters[9].map = Map.MAP4;
            monsters[9].firstAttack = true;
        }
    }

    class Weapon
    {
        public string name;
        public int plusDamage; // upgrade의 레벨 포함
        public int upgradeSuccess;
        public int upgradeLevel;

        public Weapon (string name, int plusDamage)
        {
            Upgrade upgrade = new Upgrade();
            this.name = name;
            this.plusDamage = plusDamage;
            upgradeLevel = 0;
            upgradeSuccess = upgrade.level[upgradeLevel];
        }
    }

    class Job
    {
        public string name = String.Empty;
        public int plusHp;
        public int plusAttack;
        public JobEnum jobEnum;

        public Job(string name, int plusHp, int plusAttack, JobEnum jobEnum)
        {
            this.name = name;
            this.plusHp = plusHp;
            this.plusAttack = plusAttack;
            this.jobEnum = jobEnum;
        }
    }

    class Skill
    {
        public string name;
        public int attck;
        public int cooldown; // 쿨타임
        public bool used = false; // 스킬 사용 했는지

        public Skill(string name, int attack, int cooldown)
        {
            this.name = name;
            this.attck = attack;
            this.cooldown = cooldown;
        }
    }
}
