
using System.Security.Cryptography.X509Certificates;
using static Program;
public class Program
{
    public static void Main(string[] args)
    {
        robot_penyerang robotA = new robot_penyerang("Robot A", 100, 10, 20);
        bos_robot bos = new bos_robot("Bos Robot", 100, 10, 25, 20);

        // Kemampuan robot
        Iskill repair = new Repair(3);
        Iskill electricShock = new ElectricShock(2);
        PlasmaCannon plasmaCannon = new PlasmaCannon(3);
        pertahanan_super bertahan = new pertahanan_super(3, 15, 3); 

        while (!robotA.mati() && !bos.mati())
        {
            robotA.CetakInformasi();
            bos.CetakInformasi();

            Console.WriteLine("Pilih aksi untuk Robot A:");
            Console.WriteLine("1. Serang");
            Console.WriteLine("2. Gunakan kekuatan");
            Console.Write("Pilihan: ");

            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    Console.Clear();
                    robotA.serang(bos);
                    break;
                case "2":
                    Console.Clear();
                    Console.WriteLine("1. Gunakan Repair");
                    Console.WriteLine("2. Gunakan Electric Shock");
                    Console.WriteLine("3. Gunakan Plasma Cannon");
                    Console.WriteLine("4. Gunakan Pertahanan Super");
                    Console.Write("Pilihan: ");
                    string serang = Console.ReadLine();
                    switch (serang)
                    {
                        case "1":
                            Console.Clear();
                            robotA.pakekemampuan(repair, robotA);
                            break;
                        case "2":
                            Console.Clear();
                            robotA.pakekemampuan(electricShock, bos);
                            break;
                        case "3":
                            Console.Clear();
                            robotA.pakekemampuan(plasmaCannon, bos);
                            break;
                        case "4":
                            Console.Clear();
                            robotA.pakekemampuan(bertahan, robotA);
                            break;
                        default:
                            Console.Clear();
                            Console.WriteLine("Pilihan tidak valid.");
                            continue;
                    }
                    break;
                default:
                    Console.Clear();
                    Console.WriteLine("Pilihan tidak valid.");
                    continue;
            }

            (repair as Repair)?.recalcooldown();
            (electricShock as ElectricShock)?.recalcooldown();
            (plasmaCannon as PlasmaCannon)?.recalcooldown();
            (bertahan as pertahanan_super)?.recalcooldown();
            if (!bos.mati())
            {
                bos.serang(robotA);
            }
            if (robotA.mati())
            {
                robotA.ismati();
            }
            if (bos.mati())
            {
                bos.ismati();
            }
        }
  }
public abstract class robot
    {
        public string nama { get; set; }
        public int energi { get; set; }
        public int armor { get; set; }
        public int serangan { get; set; }
        public robot(string nama, int energi, int armor, int serangan)
        {
            this.nama = nama;
            this.energi = energi;
            this.armor = armor;
            this.serangan = serangan;
        }
        public void serang(robot target)
        {
            int damage = Math.Max(serangan - target.armor, 0);
            target.energi -= damage;
            Console.WriteLine($"{nama} menyerang {target.nama} dengan serangan {damage} poin.");
        }
        public bool mati()
        {
            return energi <= 0;
        }

        public void CetakInformasi()
        {
            Console.WriteLine($"\nNama: {nama}");
            Console.WriteLine($"Energi: {energi}");
            Console.WriteLine($"Armor: {armor}");
            Console.WriteLine($"Serangan: {serangan}\n");
        }

    }
    public interface Iskill
    {
        void gunakan(robot target);
        int cooldownselesai();
    }
    public class bos_robot : robot
    {
        public int pertahanan { get; set; }
        public bos_robot(string nama, int energi, int armor, int serangan, int pertahanan) : base(nama, energi, armor, serangan)
        {
            this.pertahanan = pertahanan;
        }

        public void Diserang(robot penyerang)
        {
            int damage = Math.Max(penyerang.serangan - this.pertahanan, 0);
            energi -= damage;
            Console.WriteLine($"{nama} diserang oleh {penyerang.nama} dengan serangan {damage} poin.");
        }
        public void ismati()
        {
            if (mati())
            {
                Console.WriteLine($"{nama} telah kalah dalam pertarungan!");
            }
        }

    }
    public class robot_penyerang : robot
    {
        public robot_penyerang(string nama, int energi, int armor, int serangan)
            : base(nama, energi, armor, serangan) { }
        public void pakekemampuan(Iskill kemampuan, robot target)
        {
            if (energi > 0)
            {
                kemampuan.gunakan(target);
            }
        }
        public void ismati()
        {
            if (mati())
            {
                Console.WriteLine($"{nama} telah kalah dalam pertarungan!");
            }
        }
    }
    public class Repair : Iskill
    {
        private int cooldown;
        private int cekcooldown = 0;

        public Repair(int cooldown)
        {
            this.cooldown = cooldown;
        }

        public void gunakan(robot target)
        {
            if (cooldownselesai() == 0)
            {
                target.energi += 20;
                Console.WriteLine($"{target.nama} menggunakan Repair. Energinya bertambah 20.");
                cekcooldown = cooldown;
            }
            else
            {
                Console.WriteLine("Repair masih dalam cooldown.");
            }
        }

        public int cooldownselesai()
        {
            return cekcooldown;
        }

        public void recalcooldown()
        {
            if (cekcooldown > 0)
            {
                cekcooldown--;
            }
        }
    }
    public class ElectricShock : Iskill
    {
        private int cooldown;
        private int cekcooldown = 0;

        public ElectricShock(int cooldown)
        {
            this.cooldown = cooldown;
        }

        public void gunakan(robot target)
        {
            if (cooldownselesai() == 0)
            {
                target.energi -= 15;
                Console.WriteLine($"{target.nama} terkena Electric Shock. Energinya berkurang 15.");
                cekcooldown = cooldown;
            }
            else
            {
                Console.WriteLine("Electric Shock masih dalam cooldown.");
            }
        }

        public int cooldownselesai()
        {
            return cekcooldown;
        }

        public void recalcooldown()
        {
            if (cekcooldown > 0)
            {
                cekcooldown--;
            }
        }
    }
    public class PlasmaCannon : Iskill
    {
        private int cooldown;
        private int cekcooldown = 0;

        public PlasmaCannon(int cooldown)
        {
            this.cooldown = cooldown;
        }

        public void gunakan(robot target)
        {
            if (cooldownselesai() == 0)
            {
                target.energi -= 30;
                Console.WriteLine($"{target.nama} terkena serangan Plasma Cannon. Energinya berkurang 30 tanpa mempertimbangkan armor.");
                cekcooldown = cooldown;
            }
            else
            {
                Console.WriteLine("Plasma Cannon masih dalam cooldown.");
            }
        }

        public int cooldownselesai()
        {
            return cekcooldown;
        }

        public void recalcooldown()
        {
            if (cekcooldown > 0)
            {
                cekcooldown--;
            }
        }
    }

    public class pertahanan_super : Iskill
    {
        private int cooldown;
        private int cekcooldown = 0;
        private int tambahanArmor;
        private int durasi;

        public pertahanan_super(int cooldown, int tambahanArmor, int durasi)
        {
            this.cooldown = cooldown;
            this.tambahanArmor = tambahanArmor;
            this.durasi = durasi;
        }

        public void gunakan(robot target)
        {
            if (cooldownselesai() == 0)
            {
                target.armor += tambahanArmor;
                Console.WriteLine($"{target.nama} menggunakan Super Shield. Armor bertambah {tambahanArmor} selama {durasi} putaran.");
                cekcooldown = cooldown;
                System.Threading.Tasks.Task.Delay(durasi * 1000).ContinueWith(t => target.armor -= tambahanArmor);
            }
            else
            {
                Console.WriteLine("Super Shield masih dalam cooldown.");
            }
        }

        public int cooldownselesai()
        {
            return cekcooldown;
        }

        public void recalcooldown()
        {
            if (cekcooldown > 0)
            {
                cekcooldown--;
            }
        }
    } 
}
