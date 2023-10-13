using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Reflection.Emit;

namespace _20230907_ConsoleProject
{
    public enum Menu
    {
        START = 1,
        END
    }

    public enum MapMenu
    {
        MAP = 1,
        UPGRADE,
        RANDOM,
    }

    public enum Map
    {
        MAP1 = 1,
        MAP2,
        MAP3,
        MAP4,
        BACK
    }

    public enum UpgradeMenu
    {
        STARO = 1,
        STARX,
    }

    public enum ContinueMenu
    {
        CONTINUE = 1,
        END
    }

    class UI
    {
        public static bool end = true; // 게임 종료시 false로 바꿈
        ConsoleKeyInfo cki;
        char leftCursor = '▶';
        char rightCursor = '◀';

        public static void ClearCurrentLine()
        { // 한줄만 지우는 함수
            Thread.Sleep(1000);
            string s = "\r";
            s += new string(' ', Console.CursorLeft);
            s += "\r";
            Console.Write(s);
        }

        public static void PlayerState(Player player)
        { // 맵&강화 메뉴 선택 시 뜸
            Console.WriteLine($"======[현재 {player.name} 상태]======");
            Console.WriteLine($"　Lv. {player.level} 직업: {player.job.name}");
            Console.WriteLine($"　{player.weapon.name} 상태: ★{player.weapon.upgradeLevel}");
            Console.WriteLine($"　체력: {player.hp} 공격력: {player.attack}");
            Console.WriteLine($"　경험치: {player.exp} 돈: {player.money}");
            Console.Write("========================");
            for (int i = 0; i < player.name.Length; i++)
            {
                Console.Write("=");
            }
            Console.WriteLine();
        }

        public static void PlayerBattleState(Player player, Monster monster)
        { // 맵 선택 후 배틀 시작할 때 뜸
            Console.WriteLine($"======[{player.name} 상태]======│ ======[{monster.name} 상태]======");
            Console.Write($"　Lv. {player.level} 직업: {player.job.name}");
            Console.WriteLine($"　　　체력: {monster.hp} 공격력: {monster.attack}");
            Console.WriteLine($"　{player.weapon.name} 상태: ★{player.weapon.upgradeLevel}");
            Console.WriteLine($"　체력: {player.hp} 공격력: {player.attack}");
            Console.WriteLine($"　경험치: {player.exp} 돈: {player.money}");
            Console.WriteLine("========================================================");
        }

        public void StartSelect(Player player)
        { // 시작 메뉴 선택
            int i = 1;
            Console.WriteLine("[게임 시작]\n");
            Console.Write("{0, 0}시작하기{1, -6}종료하기", leftCursor, rightCursor);
            Console.WriteLine();
            while (true)
            {
                
                cki = Console.ReadKey(true);
                if (cki.Key.Equals(ConsoleKey.RightArrow))
                {
                    if (i == 1)
                    {
                        Console.Clear();
                        i = 2;
                        Console.WriteLine("[게임 시작]\n");
                        Console.Write("　시작하기{0, 6}종료하기{1, 0}\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (cki.Key.Equals(ConsoleKey.LeftArrow))
                {
                    if (i == 2)
                    {
                        Console.Clear();
                        i = 1;
                        Console.WriteLine("[게임 시작]\n");
                        Console.Write("{0, 0}시작하기{1, -6}종료하기\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                if (cki.Key.Equals(ConsoleKey.Enter))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            switch ((Menu)i)
            {
                case Menu.START: // 이름을 입력하면 직업 선택으로 넘어감
                    Console.Write("\n[이름 입력]\n플레이어 이름을 입력해주세요\n");
                    while (true)
                    {
                        player.name = Console.ReadLine();
                        if (player.name == String.Empty)
                        {
                            Console.Write("이름을 다시 입력해주세요.");
                            UI.ClearCurrentLine();
                        } else if (player.name == "jae")
                        {
                            Console.Clear();
                            player.PlayerJobSetting(JobEnum.JOB1);
                            player.money += 10000000;
                            player.attack = 1000;
                            player.hp = 5000;
                            player.skill[(int)SkillEnum.ATTACK].attck = player.attack + player.weapon.plusDamage;
                            player.skill[(int)SkillEnum.HEAL].attck = 2 * player.weapon.plusDamage;
                            player.skill[(int)SkillEnum.SKILL].attck = player.attack + player.weapon.plusDamage;
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            JobMenuSelect(player); // 직업 선택 메뉴
                            break;
                        }
                    }
                    break;
                case Menu.END: // 게임 종료
                    UI.end = false;
                    break;
            }
        }

        public void MapMenuSelect(Player player, Monster[] monsters, Upgrade upgrade)
        { // 직업 선택 후 메뉴 선택
            int i = 1;
            char c = ' ';
            PlayerState(player);
            Console.WriteLine("[메뉴 선택]\n");
            Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
            Console.WriteLine();
            while (true)
            {
                cki = Console.ReadKey(true);
                if (cki.Key.Equals(ConsoleKey.RightArrow))
                {
                    if (i == 1)
                    {
                        Console.Clear();
                        i = 2;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("　모험{0, 6}강화{1, -6}뽑기\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else if (i == 2)
                    {
                        Console.Clear();
                        i = 3;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("　모험{0, 6}강화{1, 6}뽑기{2, 0}\n", c, leftCursor, rightCursor);
                        Console.WriteLine();
                    } else
                    { // i== 3
                        continue;
                    }
                }
                else if (cki.Key.Equals(ConsoleKey.LeftArrow))
                {
                    if (i == 3)
                    {
                        Console.Clear();
                        i = 2;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("　모험{0, 6}강화{1, -6}뽑기\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else if (i == 2)
                    {
                        Console.Clear();
                        i = 1;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                if (cki.Key.Equals(ConsoleKey.Enter))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            switch ((MapMenu)i)
            {
                case MapMenu.MAP:
                    MapSelect(player, monsters, upgrade); // 맵 선택 메뉴
                    break;
                case MapMenu.UPGRADE:
                    upgrade.UpgradeCatch(player, monsters, upgrade); // 강화 선택 메뉴
                    break;
                case MapMenu.RANDOM: // 뽑기 메뉴
                    upgrade.RandomWeapon(player, monsters, upgrade);
                    break;
            }
        }

        public void MapSelect(Player player, Monster[] monsters, Upgrade upgrade)
        { // 맵 메뉴 선택 후 맵 선택
            int i = 1;
            char c = ' ';
            Console.WriteLine("[맵 선택]\n");
            Console.Write("{0, 0}Map1{1, -6}Map2{2, 6}Map3{3, 6}Map4{4, 6}돌아가기\n", leftCursor, rightCursor, c, c, c);
            Console.WriteLine();
            while (true)
            {
                cki = Console.ReadKey(true);
                if (cki.Key.Equals(ConsoleKey.RightArrow))
                {
                    if (i == 1)
                    {
                        Console.Clear();
                        i = 2;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                        Console.WriteLine("[맵 선택]\n");
                        Console.Write("　Map1{0, 6}Map2{1, -6}Map3{2, 6}Map4{3, 6}돌아가기\n", leftCursor, rightCursor, c, c);
                        Console.WriteLine();
                    }
                    else if (i == 2)
                    {
                        Console.Clear();
                        i = 3;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                        Console.WriteLine("[맵 선택]\n");
                        Console.Write("　Map1{0, 6}Map2{1, 6}Map3{2, -6}Map4{3, 6}돌아가기\n", c, leftCursor, rightCursor, c);
                        Console.WriteLine();
                    } else if (i == 3)
                    {
                        Console.Clear();
                        i = 4;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                        Console.WriteLine("[맵 선택]\n");
                        Console.Write("　Map1{0, 6}Map2{1, 6}Map3{2, 6}Map4{3, -6}돌아가기\n", c, c, leftCursor, rightCursor, c);
                        Console.WriteLine();
                    } else if (i == 4)
                    { // i == 4
                        Console.Clear();
                        i = 5;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                        Console.WriteLine("[맵 선택]\n");
                        Console.Write("　Map1{0, 6}Map2{1, 6}Map3{2, 6}Map4{3, 6}돌아가기{4, 0}\n", c, c, c, leftCursor, rightCursor);
                        Console.WriteLine();
                    } else
                    {
                        continue;
                    }
                }
                else if (cki.Key.Equals(ConsoleKey.LeftArrow))
                {
                    if (i == 1)
                    {
                        continue;
                        
                    }
                    else if (i == 2)
                    {
                        Console.Clear();
                        i = 1;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                        Console.WriteLine("[맵 선택]\n");
                        Console.Write("{0, 0}Map1{1, -6}Map2{2, 6}Map3{3, 6}Map4{4, 6}돌아가기\n", leftCursor, rightCursor, c, c, c);
                        Console.WriteLine();
                    } else if (i == 3)
                    {
                        Console.Clear();
                        i = 2;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                        Console.WriteLine("[맵 선택]\n");
                        Console.Write("　Map1{0, 6}Map2{1, -6}Map3{2, 6}Map4{3, 6}돌아가기\n", leftCursor, rightCursor, c, c);
                        Console.WriteLine();
                    } else if (i == 4)
                    { // i == 4
                        Console.Clear();
                        i = 3;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                        Console.WriteLine("[맵 선택]\n");
                        Console.Write("　Map1{0, 6}Map2{1, 6}Map3{2, -6}Map4{3, 6}돌아가기\n", c, leftCursor, rightCursor, c);
                        Console.WriteLine();
                    } else
                    { // i == 5
                        Console.Clear();
                        i = 4;
                        PlayerState(player);
                        Console.WriteLine("[메뉴 선택]\n");
                        Console.Write("{0, 0}모험{1, -6}강화{2, 6}뽑기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                        Console.WriteLine("[맵 선택]\n");
                        Console.Write("　Map1{0, 6}Map2{1, 6}Map3{2, 6}Map4{3, -6}돌아가기\n", c, c, leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                }
                if (cki.Key.Equals(ConsoleKey.Enter))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            
            Random rand = new Random();

            switch ((Map)i)
            {
                case Map.MAP1:
                    BattleSetting(player, monsters, upgrade, rand.Next(0, 3));
                    break;
                case Map.MAP2:
                    BattleSetting(player, monsters, upgrade, rand.Next(3, 6));
                    break;
                case Map.MAP3:
                    BattleSetting(player, monsters, upgrade, rand.Next(6, 9));
                    break;
                case Map.MAP4:
                    BattleSetting(player, monsters, upgrade, 9);
                    break;
                case Map.BACK:
                    Console.WriteLine("메뉴 화면으로 돌아갑니다.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    MapMenuSelect(player, monsters, upgrade);
                    break;
            }
        }

        public void BattleSetting(Player player, Monster[] monsters, Upgrade upgrade, int random)
        {
            UI select = new UI();
            Player playerNow = player; // 플레이어의 현재 정보 저장
            Monster monsterNow = monsters[random];
            bool notDamage = true; // 데미지를 입지 않을 때 true, 입었을 때 false
            bool monsterAlive = true; // 플레이어가 살았을 때 false, 몬스터가 살았을 때 true
            string s = String.Empty; // 줄줄이 출력해 줄 것을 담는 string
            monsterNow.MonsterSetting(monsters);
            // 새로운 판마다 스킬 쿨타임 초기화
            for (int i = 0; i < player.skill.Length; i++)
            {
                player.skill[i].used = false;
            }
            player.skill[(int)SkillEnum.HEAL].cooldown = 1;
            player.skill[(int)SkillEnum.SKILL].cooldown = 2;

            Console.WriteLine($"{monsterNow.name}이 나타났습니다!");
            Thread.Sleep(1000);
            Console.Clear();
            ConsoleKeyInfo cki;

            while (true)
            {
                if (monsterNow.firstAttack == true)
                {
                    while (true)
                    {
                        Console.Clear();
                        UI.PlayerBattleState(player, monsterNow);
                        Console.WriteLine("[공격 선택]\n");
                        Console.WriteLine("기본 공격(Q) | 회복(W) | 강화 공격(E) | 도망치기(R)\n");
                        Console.Write(s);

                        cki = Console.ReadKey(true);
                        if (cki.Key == ConsoleKey.Q)
                        { // 기본 공격
                            string plusS = $"{player.name}은 {monsterNow.attack}만큼의 데미지를 입었다.\n{player.name}은 {player.skill[(int)SkillEnum.ATTACK].name}를 사용했다.\n{monsterNow.name}은 {player.skill[(int)SkillEnum.ATTACK].attck}만큼의 데미지를 입었다.\n";
                            s += plusS;
                            player.hp -= monsterNow.attack;
                            monsterNow.hp -= player.skill[(int)SkillEnum.ATTACK].attck;
                            player.skill[(int)SkillEnum.ATTACK].used = true;
                            player.skill[(int)SkillEnum.ATTACK].cooldown = 0;
                            for (int i = 0; i < player.skill.Length; i++)
                            {
                                if (player.skill[i].cooldown == playerNow.skill[i].cooldown)
                                {
                                    player.skill[i].used = false;
                                }
                                else
                                {
                                    if (player.skill[i].used == true)
                                    {
                                        player.skill[i].cooldown++;
                                    }

                                }
                            }
                        }
                        else if (cki.Key == ConsoleKey.W)
                        { // 회복
                            if (player.skill[(int)SkillEnum.HEAL].used == true)
                            {
                                string plusS = $"현재 스킬을 사용할 수 없다.\n";
                                s += plusS;
                            }
                            else
                            {
                                string plusS = $"{player.name}은 {monsterNow.attack}만큼의 데미지를 입었다.\n{player.name}은 {player.skill[(int)SkillEnum.HEAL].name}를 사용했다.\n{player.name}은 {player.skill[(int)SkillEnum.HEAL].attck}만큼 회복했다.\n";
                                s += plusS;
                                player.hp -= monsterNow.attack;
                                player.hp += player.skill[(int)SkillEnum.HEAL].attck;
                                if (player.hp >= 5000)
                                {
                                    player.hp = 5000;
                                }
                                player.skill[(int)SkillEnum.HEAL].used = true;
                                player.skill[(int)SkillEnum.HEAL].cooldown = 0;
                                for (int i = 0; i < player.skill.Length; i++)
                                {
                                    if (i != 1)
                                    {
                                        if (player.skill[i].cooldown == playerNow.skill[i].cooldown)
                                        {
                                            player.skill[i].used = false;
                                        }
                                        else
                                        {
                                            if (player.skill[i].used == true)
                                            {
                                                player.skill[i].cooldown++;
                                            }
                                        }
                                    }
                                }
                            }

                        }
                        else if (cki.Key == ConsoleKey.E)
                        { // 강화 공격
                            if (player.skill[(int)SkillEnum.SKILL].used == true)
                            {
                                string plusS = $"현재 스킬을 사용할 수 없다.\n";
                                s += plusS;
                            }
                            else
                            {
                                string plusS = $"{player.name}은 {monsterNow.attack}만큼의 데미지를 입었다.\n{player.name}은 {player.skill[(int)SkillEnum.SKILL].name}를 사용했다.\n{monsterNow.name}은 {player.skill[(int)SkillEnum.SKILL].attck}만큼의 데미지를 입었다.\n";
                                s += plusS;
                                player.hp -= monsterNow.attack;
                                monsterNow.hp -= player.skill[(int)SkillEnum.ATTACK].attck;
                                player.skill[(int)SkillEnum.SKILL].used = true;
                                player.skill[(int)SkillEnum.SKILL].cooldown = 0;
                                for (int i = 0; i < player.skill.Length; i++)
                                {
                                    if (i != 2)
                                    {
                                        if (player.skill[i].cooldown == playerNow.skill[i].cooldown)
                                        {
                                            player.skill[i].used = false;
                                        }
                                        else
                                        {
                                            if (player.skill[i].used == true)
                                            {
                                                player.skill[i].cooldown++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (cki.Key == ConsoleKey.R)
                        { // 도망치기
                            Random rand = new Random();
                            if (rand.Next(0, 101) <= 50)
                            { // 도망칠 수 있는 확률 반반
                                string plusS = $"{player.name}은 {monsterNow.attack}만큼의 데미지를 입었다.\n현재 도망칠 수 없다.\n";
                                s += plusS;
                                player.hp -= monsterNow.attack;
                                for (int i = 0; i < player.skill.Length; i++)
                                {
                                    if (i != 2)
                                    {
                                        if (player.skill[i].cooldown == playerNow.skill[i].cooldown)
                                        {
                                            player.skill[i].used = false;
                                        }
                                        else
                                        {
                                            if (player.skill[i].used == true)
                                            {
                                                player.skill[i].cooldown++;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string plusS = "무사히 도망쳤다.\n";
                                Console.Write(plusS);
                                Thread.Sleep(1000);
                                Console.Clear();
                                monsterNow.hp = 0;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("잘못 입력했습니다. 다시 입력해주세요.");
                            UI.ClearCurrentLine();
                        }

                        if (monsterNow.hp <= 0)
                        {
                            monsterNow.DeadMonster(player);
                            monsterAlive = false;
                            Thread.Sleep(1000);
                            Console.Clear();
                            break;
                        }
                        else
                        {
                            Thread.Sleep(1000);
                            Console.Clear();
                        }
                        if (player.hp <= 0)
                        {
                            monsterAlive = true;
                            break;
                        }
                    }
                    if (!monsterAlive)
                    {
                        player.PlayerLevelUp(player.skill);
                        break;
                    }
                    else
                    { // 플레이어가 죽었을 시
                        break;
                    }
                }
                else
                { // 플레이어 선공
                    while (true)
                    {
                        Console.Clear();
                        UI.PlayerBattleState(player, monsterNow);
                        Console.WriteLine("[공격 선택]\n");
                        Console.WriteLine("기본 공격(Q) | 회복(W) | 강화 공격(E) | 도망치기(R)\n");
                        Console.Write(s);
                        cki = Console.ReadKey(true);
                        if (cki.Key == ConsoleKey.Q)
                        { // 기본 공격
                            string plusS = $"{player.name}은 {player.skill[(int)SkillEnum.ATTACK].name}를 사용했다.\n{monsterNow.name}은 {player.skill[(int)SkillEnum.ATTACK].attck}만큼의 데미지를 입었다.\n";
                            s += plusS;
                            monsterNow.hp -= player.skill[(int)SkillEnum.ATTACK].attck;
                            player.skill[(int)SkillEnum.ATTACK].used = true;
                            player.skill[(int)SkillEnum.ATTACK].cooldown = 0;
                            notDamage = false;
                            for (int i = 0; i < player.skill.Length; i++)
                            {
                                if (player.skill[i].cooldown == playerNow.skill[i].cooldown)
                                {
                                    player.skill[i].used = false;
                                }
                                else
                                {
                                    if (player.skill[i].used == true)
                                    {
                                        player.skill[i].cooldown++;
                                    }
                                }

                            }
                        }
                        else if (cki.Key == ConsoleKey.W)
                        { // 회복
                            if (player.skill[(int)SkillEnum.HEAL].used == true)
                            {
                                string plusS = $"현재 스킬을 사용할 수 없다.\n";
                                s += plusS;
                                notDamage = true;
                            }
                            else
                            {
                                string plusS = $"{player.name}은 {player.skill[(int)SkillEnum.HEAL].name}를 사용했다.\n{player.name}은 {player.skill[(int)SkillEnum.HEAL].attck}만큼 회복했다.\n";
                                s += plusS;
                                player.hp += player.skill[(int)SkillEnum.HEAL].attck;
                                if (player.hp >= 5000)
                                {
                                    player.hp = 5000;
                                }
                                player.skill[(int)SkillEnum.HEAL].used = true;
                                player.skill[(int)SkillEnum.HEAL].cooldown = 0;
                                notDamage = false;
                                for (int i = 0; i < player.skill.Length; i++)
                                {
                                    if (i != 1)
                                    {
                                        if (player.skill[i].cooldown == playerNow.skill[i].cooldown)
                                        {
                                            player.skill[i].used = false;
                                        }
                                        else
                                        {
                                            if (player.skill[i].used == true)
                                            {
                                                player.skill[i].cooldown++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (cki.Key == ConsoleKey.E)
                        { // 강화 공격
                            if (player.skill[(int)SkillEnum.SKILL].used == true)
                            {
                                string plusS = $"현재 스킬을 사용할 수 없다.\n";
                                s += plusS;
                                notDamage = true;
                            }
                            else
                            {
                                string plusS = $"{player.name}은 {player.skill[(int)SkillEnum.SKILL].name}를 사용했다.\n{monsterNow.name}은 {player.skill[(int)SkillEnum.SKILL].attck}만큼의 데미지를 입었다.\n";
                                s += plusS;
                                Console.WriteLine(s);
                                monsterNow.hp -= player.skill[(int)SkillEnum.ATTACK].attck;
                                player.skill[(int)SkillEnum.SKILL].used = true;
                                player.skill[(int)SkillEnum.SKILL].cooldown = 0;
                                notDamage = false;
                                for (int i = 0; i < player.skill.Length; i++)
                                {
                                    if (i != 2)
                                    {
                                        if (player.skill[i].cooldown == playerNow.skill[i].cooldown)
                                        {
                                            player.skill[i].used = false;
                                        }
                                        else
                                        {
                                            if (player.skill[i].used == true)
                                            {
                                                player.skill[i].cooldown++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        else if (cki.Key == ConsoleKey.R)
                        { // 도망치기
                            notDamage = true;
                            Random rand = new Random();
                            if (rand.Next(0, 101) <= 50)
                            { // 도망칠 수 있는 확률 반반
                                string plusS = $"{player.name}은 현재 도망칠 수 없다.\n";
                                s += plusS;
                                Console.WriteLine(s); for (int i = 0; i < player.skill.Length; i++)
                                {
                                    if (i != 2)
                                    {
                                        if (player.skill[i].cooldown == playerNow.skill[i].cooldown)
                                        {
                                            player.skill[i].used = false;
                                        }
                                        else
                                        {
                                            if (player.skill[i].used == true)
                                            {
                                                player.skill[i].cooldown++;
                                            }
                                        }
                                    }
                                }
                            }
                            else
                            {
                                string plusS = "무사히 도망쳤다.\n";
                                s += plusS;
                                Console.WriteLine(s);
                                Thread.Sleep(1000);
                                Console.Clear();
                                monsterNow.hp = 0;
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine("잘못 입력했습니다. 다시 입력해주세요.");
                            UI.ClearCurrentLine();
                            notDamage = true;
                        }

                        if (!notDamage && monsterAlive)
                        {
                            Console.WriteLine($"{player.name}은 {monsterNow.attack}만큼의 데미지를 입었다.");
                            player.hp -= monsterNow.attack;
                        }

                        if (monsterNow.hp <= 0)
                        {
                            monsterNow.DeadMonster(player);
                            monsterAlive = false;
                            Thread.Sleep(3000);
                            Console.Clear();
                            break;
                        }
                        else if (player.hp <= 0)
                        {
                            monsterAlive = true;
                            break;
                        }
                    }
                    if (!monsterAlive)
                    {
                        player.PlayerLevelUp(player.skill);
                        break;
                    }
                    else
                    { // 플레이어가 죽었을 시
                        break;
                    }
                }
            }

            if (player.hp > 0)
            {
                if (random == 9)
                {
                    // Clear
                    if (monsterAlive)
                    {
                        select.MapMenuSelect(player, monsters, upgrade);
                    }
                    else
                    {
                        Clear();
                        end = true;
                        select.StartSelect(player); // 처음으로
                    }
                } else
                {
                    select.MapMenuSelect(player, monsters, upgrade);
                }
            }
            else
            {
                Thread.Sleep(1000);
                Console.Clear();
                Console.WriteLine("플레이어의 체력이 0이 되었습니다.\n");
                ContinueSelect(player, monsters, upgrade);
            }
        }

        public void Clear()
        {
            Console.WriteLine("게임 클리어! 멋져요~");
            Thread.Sleep(3000);
            Console.Clear();
        }

        public void JobMenuSelect(Player player)
        { // 시작 메뉴 선택 후 직업 선택
            int i = 1;
            Console.WriteLine("[직업 선택]\n");
            Console.Write("{0, 0}Job1{1, -6}Job2\n", leftCursor, rightCursor);
            Console.WriteLine();
            while (true)
            {
                cki = Console.ReadKey(true);
                if (cki.Key.Equals(ConsoleKey.RightArrow))
                {
                    if (i == 1)
                    {
                        Console.Clear();
                        i = 2;
                        Console.WriteLine("[직업 선택]\n");
                        Console.Write("　Job1{0, 6}Job2{1, 0}\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (cki.Key.Equals(ConsoleKey.LeftArrow))
                {
                    if (i == 2)
                    {
                        Console.Clear();
                        i = 1;
                        Console.WriteLine("[직업 선택]\n");
                        Console.Write("{0, 0}Job1{1, -6}Job2\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                if (cki.Key.Equals(ConsoleKey.Enter))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            
            switch ((JobEnum)i)
            {
                case JobEnum.JOB1:
                    Console.WriteLine("Job1을 선택했습니다.");
                    player.PlayerJobSetting(JobEnum.JOB1); // 플레이어 셋팅
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
                case JobEnum.JOB2:
                    Console.WriteLine("Job2을 선택했습니다.");
                    player.PlayerJobSetting(JobEnum.JOB2); // 플레이어 셋팅
                    Thread.Sleep(1000);
                    Console.Clear();
                    break;
            }
        }

        public void ContinueSelect(Player player, Monster[] monster, Upgrade upgrade)
        { // 배틀이 끝난 후 플레이어가 죽었을 때 선택, 사용 할 말 고민
            int i = 1;
            Console.WriteLine("[선택]\n");
            Console.Write("{0, 0}이어하기{1, -6}처음으로\n", leftCursor, rightCursor);
            Console.WriteLine();
            while (true)
            {
                cki = Console.ReadKey(true);
                if (cki.Key.Equals(ConsoleKey.RightArrow))
                {
                    if (i == 1)
                    {
                        Console.Clear();
                        i = 2;
                        Console.WriteLine("플레이어의 체력이 0이 되었습니다.\n");
                        Console.WriteLine("[선택]\n");
                        Console.Write("　이어하기{0, 6}처음으로{1, 0}\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (cki.Key.Equals(ConsoleKey.LeftArrow))
                {
                    if (i == 2)
                    {
                        Console.Clear();
                        i = 1;
                        Console.WriteLine("플레이어의 체력이 0이 되었습니다.\n");
                        Console.WriteLine("[선택]\n");
                        Console.Write("{0, 0}이어하기{1, -6}처음으로\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                if (cki.Key.Equals(ConsoleKey.Enter))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }

            switch ((ContinueMenu)i)
            {
                case ContinueMenu.CONTINUE: // 이어하기 -> 맵 선택창으로
                    Console.Clear();
                    // 플레이어 부활
                    if (player.job.jobEnum == JobEnum.JOB1)
                    {
                        player.hp = 50;
                    }
                    else
                    {
                        player.hp = 40;
                    }
                    for (int j = 1; j < player.level; j++)
                    {
                        player.hp += 100;
                    }
                    // 랜덤 패널티
                    Random rand = new Random();
                    if (rand.Next(0, 101) <= 50)
                    { // 템 강화가 하락 될 수도 있고 아닐수도ㅎ
                        if (player.weapon.upgradeLevel >= 1)
                        { // 강화가 1렙 이상일 때
                            player.weapon.upgradeLevel--;
                        }
                    }
                    MapMenuSelect(player, monster, upgrade);
                    break;
                case ContinueMenu.END: // 새로하기 -> 게임 시작 창으로, 플레이어 초기화
                    StartSelect(player);
                    break;
            }
        }
    }

    class Upgrade
    {
        UI select = new UI();
        ConsoleKeyInfo cki;
        char leftCursor = '▶';
        char rightCursor = '◀';
        public int[] level = new int[10] { 90, 80, 70, 60, 50, 35, 20, 10, 5, 1 }; // 레벨 당 성공 확률
        public int[] levelFail = new int[10] { 0, 0, 0, 5, 7, 10, 13, 15, 20, 25 }; // 레벨 당 터질 확률
        public int[] success = new int[3] { 1, 2, 3 }; // 캐치 성공 시 추가 확률
        public double upgradeResult = 0;
        public int randomSuccess = 0;

        public void StarUI(Player player)
        {
            string bar = String.Empty;
            string success = $"강화 성공 확률: {level[player.weapon.upgradeLevel]}%";
            string money = $"강화 비용: {500 * (player.weapon.upgradeLevel + 1)} │ 남은 돈: {player.money}\n";
            char h = '│';
            Console.WriteLine("[스타캐치]");
            Console.WriteLine("┌───────────────────────────────┐");
            for (int i = 0; i < player.weapon.upgradeLevel; i++)
            {
                bar += '★';
            }
            for (int i = player.weapon.upgradeLevel; i < 10; i++)
            {
                bar += '☆';
            }
            Console.Write("{0, -6}{1, 0}{2, 7}", h, bar, h);
            Console.WriteLine();
            Console.Write("{0, -12}{1, 0}{2, 12}", h, player.weapon.name, h);
            Console.WriteLine();
            if (player.weapon.upgradeLevel <= 7)
            {
                Console.Write("{0, -8}{1, 0}{2, 6}", h, success, h);
            } else
            {
                Console.Write("{0, -8}{1, 0}{2, 7}", h, success, h);
            }
            Console.WriteLine();
            Console.WriteLine("└───────────────────────────────┘");
            Console.Write("{0, 0}", money);
        }

        public void RandomResultUI(int result)
        {
            Console.Clear();
            Console.WriteLine("[뽑기 결과]");
            Console.WriteLine("▩▩▩▩▩▩▩▩▩▩▩▩▩▩▩");
            Console.WriteLine("▩▩▩▩▩▩▩▩▩▩▩▩▩▩▩");
            Console.WriteLine($"▩▩▩▩★{result}▩▩▩▩");
            Console.WriteLine("▩▩▩▩▩▩▩▩▩▩▩▩▩▩▩");
            Console.WriteLine("▩▩▩▩▩▩▩▩▩▩▩▩▩▩▩");
        }

        public void UpgradeCatch(Player player, Monster[] monsters, Upgrade upgrade)
        {
            UI select = new UI();
            Console.Clear();
            while (true)
            {
                int check = 1;
                double successSelect = 0;
                StarUI(player);
                Console.WriteLine("스타캐치를 하시겠습니까? (진행 시 확률 증가)\n");
                Console.Write("{0, 0}체크{1, -6}체크 안함\n", leftCursor, rightCursor);

                while (true)
                {
                    cki = Console.ReadKey(true);
                    Console.Clear();
                    if (cki.Key.Equals(ConsoleKey.RightArrow))
                    {
                        if (check == 1)
                        {
                            Console.Clear();
                            check = 2;
                            StarUI(player);
                            Console.WriteLine("스타캐치를 하시겠습니까? (진행 시 확률 증가)\n");
                            Console.Write("　체크{0, 6}체크 안함{1, 0}\n", leftCursor, rightCursor);
                            Console.WriteLine();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (cki.Key.Equals(ConsoleKey.LeftArrow))
                    {
                        if (check == 2)
                        {
                            Console.Clear();
                            check = 1;
                            StarUI(player);
                            Console.WriteLine("스타캐치를 하시겠습니까? (진행 시 확률 증가)\n");
                            Console.Write("{0, 0}체크{1, -6}체크 안함\n", leftCursor, rightCursor);
                            Console.WriteLine();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (cki.Key.Equals(ConsoleKey.Enter))
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }

                if (500 * (player.weapon.upgradeLevel + 1) > player.money)
                {
                    Console.WriteLine("\n돈이 모자라 강화를 할 수 없습니다.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    select.MapMenuSelect(player, monsters, upgrade);
                    break;
                }

                // 재화 소모
                player.money -= 500 * (player.weapon.upgradeLevel + 1);

                if ((UpgradeMenu)check == UpgradeMenu.STARO)
                { // 스타캐치 선택
                    int starX = 0, cnt = 0, sign = 1;
                    string barString = "□□□□▦▦■■■▦▦□□□□";
                    StringBuilder barSwitch = new StringBuilder(barString);
                    char barToStar = '★';
                    char h = '│';
                    Console.Clear();
                    StarUI(player);
                    do
                    {
                        while (Console.KeyAvailable == false)
                        {
                            Console.WriteLine("┌───────────────────────────────┐");
                            barSwitch[starX] = barToStar; // 0 ~ 12
                            for (int i = 0; i < barString.Length; i++)
                            {
                                if (i != starX)
                                {
                                    string bar = "□□□□▦▦■■■▦▦□□□□";
                                    barSwitch[i] = bar[i];
                                }
                            }
                            barString = barSwitch.ToString();
                            Console.WriteLine("{0, -2}{1, 0}{2, 1}", h, barString, h);
                            Console.WriteLine("└───────────────────────────────┘");

                            starX += sign;
                            if (starX == 0 || starX == 14)
                            {
                                sign *= -1; // 반대로
                                cnt++;
                            }
                            Thread.Sleep(100);
                            Console.Clear();

                            if (cnt != 3)
                            { // 자동으로 끝나기 전에 Enter를 눌렀을 때, 바 위치에 따른 성공 확률 추가
                                if (3 < starX || starX < 11)
                                {
                                    successSelect = success[1];
                                    if (5 < starX || starX < 9)
                                    {
                                        successSelect = success[2];
                                    }
                                }
                                else
                                {
                                    successSelect = success[0];
                                }
                            }
                            else
                            {
                                Console.WriteLine("엔터키를 누르지 않아 자동으로 넘어갑니다.");
                                for (int i = 0; i < 15; i++)
                                {
                                    Console.Write('▷');
                                }
                                Console.WriteLine("\n결과를 보기 위해 엔터를 눌러주세요.");
                                break;
                            }

                            StarUI(player);
                        }
                        cki = Console.ReadKey(true);
                        if (cki.Key != ConsoleKey.Enter)
                        {
                            continue;
                        }
                    } while (cki.Key != ConsoleKey.Enter);
                }
                else
                { // 스타캐치 미선택
                    successSelect = success[0];
                }
                // 랜덤 확률 결과
                Random rand = new Random();
                string resultMessage = String.Empty;
                upgradeResult = rand.Next(1, 101); // level과 같거나 작으면 성공 아니면 실패
                if (level[player.weapon.upgradeLevel] + successSelect >= upgradeResult)
                { // 성공
                    Thread.Sleep(1000);
                    Console.Clear();
                    player.weapon.upgradeLevel++;
                    player.weapon.plusDamage += player.weapon.upgradeLevel * level[9 - player.weapon.upgradeLevel] / 10;
                    player.skill[(int)SkillEnum.ATTACK].attck = player.attack + player.weapon.plusDamage;
                    player.skill[(int)SkillEnum.HEAL].attck = 2 * player.weapon.plusDamage;
                    player.skill[(int)SkillEnum.SKILL].attck = player.attack + player.weapon.plusDamage;
                    StarUI(player);
                    resultMessage = "강화에 성공하였습니다.\n";
                    Console.WriteLine(resultMessage);
                }
                else
                { // 실패
                    if (rand.Next(1, 101) <= levelFail[player.weapon.upgradeLevel])
                    { // 터짐
                        if (player.weapon.upgradeLevel <= 5)
                        {
                            player.weapon.upgradeLevel = 0;
                            if (player.job.jobEnum == JobEnum.JOB1)
                            {
                                player.weapon.plusDamage = 30;
                                player.skill[(int)SkillEnum.ATTACK].attck = player.attack + player.weapon.plusDamage;
                                player.skill[(int)SkillEnum.HEAL].attck = player.attack / 2 + player.weapon.plusDamage;
                                player.skill[(int)SkillEnum.SKILL].attck = player.attack + player.weapon.plusDamage;
                            }
                            else
                            {
                                player.weapon.plusDamage = 20;
                                player.skill[(int)SkillEnum.ATTACK].attck = player.attack + player.weapon.plusDamage;
                                player.skill[(int)SkillEnum.HEAL].attck = player.attack / 2 + player.weapon.plusDamage * 2;
                                player.skill[(int)SkillEnum.SKILL].attck = player.attack + player.weapon.plusDamage;
                            }
                        }
                        else
                        { // 6레벨 이상에서 터졌을 때 5레벨로 초기화
                            player.weapon.upgradeLevel = 5;
                            for (int i = 1; i < 6; i++)
                            {
                                player.weapon.plusDamage += i * level[9 - i] / 10;
                            }
                            player.skill[(int)SkillEnum.ATTACK].attck = player.attack + player.weapon.plusDamage;
                            player.skill[(int)SkillEnum.HEAL].attck = 2 * player.weapon.plusDamage;
                            player.skill[(int)SkillEnum.SKILL].attck = player.attack + player.weapon.plusDamage;
                        }
                        Console.Clear();
                        StarUI(player);
                        resultMessage = "강화에 실패하여 장비가 사라졌습니다.\n";
                        Console.WriteLine(resultMessage);
                    }
                    else
                    { // 하락 30%
                        if (rand.Next(1, 101) <= 30)
                        { // 하락
                            if (player.weapon.upgradeLevel != 1)
                            { // 4렙이라면 4렙의 능력치를 기존에서 빼고 레벨을 하락시키면 3레벨의 웨폰, 3레벨을 가짐
                                player.weapon.plusDamage -= (player.weapon.upgradeLevel) * level[9 - player.weapon.upgradeLevel] / 10;
                                player.weapon.upgradeLevel--;
                                player.skill[(int)SkillEnum.ATTACK].attck = player.attack + player.weapon.plusDamage;
                                player.skill[(int)SkillEnum.HEAL].attck = 2 * player.weapon.plusDamage;
                                player.skill[(int)SkillEnum.SKILL].attck = player.attack + player.weapon.plusDamage;
                                Console.Clear();
                                StarUI(player);
                                resultMessage = "강화에 실패하여 장비 레벨이 하락합니다.\n";
                                Console.WriteLine(resultMessage);
                            }
                        }
                        else
                        { // 실패, 아무것도 변하지 않음
                            Console.Clear();
                            StarUI(player);
                            resultMessage = "강화에 실패하였습니다.\n";
                            Console.Write(resultMessage);
                        }
                    }
                }

                int reUpgrade = 1;
                Console.WriteLine("다시 강화를 진행하겠습니까?\n");
                Console.Write("{0, 0}예{1, -6}아니오\n", leftCursor, rightCursor);
                while (true)
                {
                    cki = Console.ReadKey(true);
                    if (cki.Key.Equals(ConsoleKey.RightArrow))
                    {
                        if (reUpgrade == 1)
                        {
                            Console.Clear();
                            reUpgrade = 2;
                            StarUI(player);
                            Console.Write(resultMessage);
                            Console.WriteLine("\n다시 강화를 진행하겠습니까?\n");
                            Console.Write("　예{0, 6}아니오{1, 0}\n", leftCursor, rightCursor);
                            Console.WriteLine();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    else if (cki.Key.Equals(ConsoleKey.LeftArrow))
                    {
                        if (reUpgrade == 2)
                        {
                            Console.Clear();
                            reUpgrade = 1;
                            StarUI(player);
                            Console.Write(resultMessage);
                            Console.WriteLine("\n다시 강화를 진행하겠습니까?\n");
                            Console.Write("{0, 0}예{1, -6}아니오\n", leftCursor, rightCursor);
                            Console.WriteLine();
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (cki.Key.Equals(ConsoleKey.Enter))
                    {
                        break;
                    }
                    else
                    {
                        continue;
                    }
                }
                if (reUpgrade == 2)
                { // 아니오 선택
                    Console.WriteLine("메뉴 화면으로 돌아갑니다.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    select.MapMenuSelect(player, monsters, upgrade);
                    break;
                }
                else
                { // 예
                    if (500 * (player.weapon.upgradeLevel + 1) > player.money)
                    {
                        Console.WriteLine("돈이 모자라 강화를 할 수 없습니다.\n메뉴 화면으로 돌아갑니다.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        select.MapMenuSelect(player, monsters, upgrade);
                        break;
                    } else
                    {
                        Thread.Sleep(1000);
                        Console.Clear();
                        continue;
                    }
                }
            }
        }

        public void RandomWeapon(Player player, Monster[] monsters, Upgrade upgrade)
        { // 웨폰 뽑기 시스템
            Console.Clear();
            double[] randomWeapon = new double[10] { 1.0, 1.5, 2.0, 2.5, 3.0, 5.0, 15.0, 20.0, 25.0, 25.0 }; // 뽑기 확률
            string bar = String.Empty;
            char h = '│';
            string money = $" 1회 비용: 2000\n 남은 돈: {player.money}";
            char c = ' ';
            // 뽑기 UI
            Console.WriteLine("[뽑기]");
            Console.WriteLine("┌───────────────────────────────┐");
            Console.Write("{0, -3}최대 ★10의 무기 획득 가능!{2, 3}", h, bar, h);
            Console.WriteLine();
            Console.WriteLine("└───────────────────────────────┘");
            Console.Write(money);
            Console.WriteLine();
            // 뽑기 선택
            int i = 1;
            Console.Write("{0, 0}1회 뽑기{1, -6}확률 확인{2, 7}돌아가기", leftCursor, rightCursor, c);
            Console.WriteLine();
            while (true)
            {
                cki = Console.ReadKey(true);
                if (cki.Key.Equals(ConsoleKey.RightArrow))
                {
                    if (i == 1)
                    {
                        Console.Clear();
                        i = 2;
                        Console.WriteLine("[뽑기]");
                        Console.WriteLine("┌───────────────────────────────┐");
                        Console.Write("{0, -3}최대 ★10의 무기 획득 가능!{2, 3}", h, bar, h);
                        Console.WriteLine();
                        Console.WriteLine("└───────────────────────────────┘");
                        Console.Write(money);
                        Console.WriteLine();
                        Console.Write("　1회 뽑기{0, 6}확률 확인{1, -6}돌아가기\n", leftCursor, rightCursor);
                        Console.WriteLine();
                        for (int j = 0; j < randomWeapon.Length; j++)
                        {
                            Console.Write($" | ★{10 - j}: {randomWeapon[j]}% | ");
                            if (j == 4)
                            {
                                Console.WriteLine();
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        Console.Clear();
                        i = 3;
                        Console.WriteLine("[뽑기]");
                        Console.WriteLine("┌───────────────────────────────┐");
                        Console.Write("{0, -3}최대 ★10의 무기 획득 가능!{2, 3}", h, bar, h);
                        Console.WriteLine();
                        Console.WriteLine("└───────────────────────────────┘");
                        Console.Write(money);
                        Console.WriteLine();
                        Console.Write("　1회 뽑기{0, 7}확률 확인{1, 6}돌아가기{2, 0}\n", c, leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (cki.Key.Equals(ConsoleKey.LeftArrow))
                {
                    if (i == 3)
                    {
                        Console.Clear();
                        i = 2;
                        Console.WriteLine("[뽑기]");
                        Console.WriteLine("┌───────────────────────────────┐");
                        Console.Write("{0, -3}최대 ★10의 무기 획득 가능!{2, 3}", h, bar, h);
                        Console.WriteLine();
                        Console.WriteLine("└───────────────────────────────┘");
                        Console.Write(money);
                        Console.WriteLine();
                        Console.Write("　1회 뽑기{0, 6}확률 확인{1, -6}돌아가기\n", leftCursor, rightCursor);
                        Console.WriteLine();
                        for (int j = 0; j < randomWeapon.Length; j++)
                        {
                            Console.Write($" | ★{10 - j}: {randomWeapon[j]}% | ");
                            if (j == 4)
                            {
                                Console.WriteLine();
                            }
                        }
                    }
                    else if (i == 2)
                    {
                        Console.Clear();
                        i = 1;
                        Console.WriteLine("[뽑기]");
                        Console.WriteLine("┌───────────────────────────────┐");
                        Console.Write("{0, -3}최대 ★10의 무기 획득 가능!{2, 3}", h, bar, h);
                        Console.WriteLine();
                        Console.WriteLine("└───────────────────────────────┘");
                        Console.Write(money);
                        Console.WriteLine();
                        Console.Write("{0, 0}1회 뽑기{1, -6}확률 확인{2, 7}돌아가기\n", leftCursor, rightCursor, c);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                if (cki.Key.Equals(ConsoleKey.Enter))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            
            switch (i)
            {
                case 1: // 뽑기
                    if (player.money < 2000)
                    {
                        Console.WriteLine("금액이 모자랍니다. 메뉴로 돌아갑니다.");
                        Thread.Sleep(1000);
                        Console.Clear();
                        select.MapMenuSelect(player, monsters, upgrade);
                    } else
                    {
                        Console.WriteLine("\n★뽑기를 시작합니다★");
                        player.money -= 2000;
                        RandomSystem(randomWeapon);
                        RandomResultUI(randomSuccess);

                    }
                    break;
                case 3:
                    Console.WriteLine("메뉴로 돌아갑니다.");
                    Thread.Sleep(1000);
                    Console.Clear();
                    select.MapMenuSelect(player, monsters, upgrade);
                    break;
            }
            // 뽑은 걸 적용할지 말지 선택
            RandomResultUI(randomSuccess);
            Console.WriteLine("결과를 적용하시겠습니까?");
            Console.Write("{0, 0}예{1, -6}아니오\n", leftCursor, rightCursor);
            int resultSelect = 1;
            while (true)
            {
                cki = Console.ReadKey(true);
                if (cki.Key.Equals(ConsoleKey.RightArrow))
                {
                    if (resultSelect == 1)
                    {
                        Console.Clear();
                        resultSelect = 2;
                        RandomResultUI(randomSuccess);
                        Console.WriteLine("결과를 적용하시겠습니까?");
                        Console.Write("　예{0, 6}아니오{1, 0}\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                else if (cki.Key.Equals(ConsoleKey.LeftArrow))
                {
                    if (resultSelect == 2)
                    {
                        Console.Clear();
                        resultSelect = 1;
                        RandomResultUI(randomSuccess);
                        Console.WriteLine("결과를 적용하시겠습니까?");
                        Console.Write("{0, 0}예{1, -6}아니오\n", leftCursor, rightCursor);
                        Console.WriteLine();
                    }
                    else
                    {
                        continue;
                    }
                }
                if (cki.Key.Equals(ConsoleKey.Enter))
                {
                    break;
                }
                else
                {
                    continue;
                }
            }
            if (resultSelect == 1)
            { // 예
                if (randomSuccess <= player.weapon.upgradeLevel)
                {
                    Console.WriteLine("현재 무기보다 같거나 낮은 등급입니다.\n적용하지 않고 넘어갑니다.");
                }  else
                {
                    int weaponLevelNow = player.weapon.upgradeLevel;
                    Console.WriteLine($"★{weaponLevelNow} -> ★{randomSuccess}");
                    for (int j = weaponLevelNow; j < randomSuccess; j++)
                    {
                        player.weapon.upgradeLevel++;
                        player.weapon.plusDamage += player.weapon.upgradeLevel * level[9 - player.weapon.upgradeLevel] / 10;
                        player.skill[(int)SkillEnum.ATTACK].attck = player.attack + player.weapon.plusDamage;
                        player.skill[(int)SkillEnum.HEAL].attck = 2 * player.weapon.plusDamage;
                        player.skill[(int)SkillEnum.SKILL].attck = player.attack + player.weapon.plusDamage;
                    }
                }
                
                Console.WriteLine("메뉴로 돌아갑니다.");
                Thread.Sleep(1000);
                Console.Clear();
                select.MapMenuSelect(player, monsters, upgrade);
            } else if (resultSelect == 2)
            { // 다시 처음으로
                RandomWeapon(player, monsters, upgrade);
            } else
            {
                select.MapMenuSelect(player, monsters, upgrade);
            }
        }

        public void RandomSystem (double[] randomWeapon)
        {
            Thread.Sleep(1000);
            Console.Clear();
            Console.WriteLine("[뽑기 결과]");
            Console.WriteLine("▩▩▩▩▩▩▩▩▩▩▩▩▩▩▩");
            Console.WriteLine("▩▩▩▩▩▩▩▩▩▩▩▩▩▩▩");
            Console.WriteLine("▩▩▩▩▩▩▩▩▩▩▩▩▩▩▩");
            Console.WriteLine("▩▩▩▩▩▩▩▩▩▩▩▩▩▩▩");
            Console.WriteLine("▩▩▩▩▩▩▩▩▩▩▩▩▩▩▩");
            Console.WriteLine("결과를 확인하기 위해 아무키나 눌러주세요.");
            cki = Console.ReadKey(true);
            Console.Clear();

            Random rand = new Random();
            int random_i = rand.Next(0, 101); // 0이상 100이하
            double random_d = 0.0;
            if (random_i != 100)
            {
                random_d = rand.NextDouble(); // 0이상 1미만
            }
            double randSum = random_i + random_d;
            double cumulative = 0.0;
            for (int i = 0; i < 10; i++)
            {
                cumulative += randomWeapon[i];
                if (randSum <= cumulative)
                {
                    this.randomSuccess = 9 - i; // 뽑기 결과 인덱스
                    break;
                }
            }
        }
    }
}