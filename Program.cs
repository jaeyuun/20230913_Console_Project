using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _20230907_ConsoleProject
{
    class Program
    {
        static void Main(string[] args)
        {
            UI select = new UI();
            Upgrade upgrade = new Upgrade();
            Monster monster = new Monster();
            Monster[] monsters = new Monster[10];
            monster.MonsterSetting(monsters);

            while (true)
            { // 시작
                Player player = new Player();
                select.StartSelect(player); // 메뉴선택 > 이름입력 > 직업선택
                if (UI.end)
                {
                    select.MapMenuSelect(player, monsters, upgrade); // 메뉴선택 > 맵 또는 업그레이드
                } 
                if (!UI.end)
                { // 게임 종료
                    Console.WriteLine("\n게임을 종료합니다.");
                    break;
                }
            }
        }
    }
}
