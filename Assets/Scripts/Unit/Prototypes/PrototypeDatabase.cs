using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeDatabase : MonoBehaviour
{
    public static PrototypeDatabase Active { protected set; get; }
    public TowerPrototype Wall;
    public TowerPrototype[] RockLauncher = new TowerPrototype[6];
    public TowerPrototype[] TeslaCoil    = new TowerPrototype[6];
    public TowerPrototype[] Generator    = new TowerPrototype[6];

    public AttackManagerPrototype AttackManagerDefault;
    public PowerManagerPrototype PowerManagerDefault;


    // Use this for initialization
    void Start()
    {
        Active = this;

        AttackManagerDefault = new AttackManagerPrototype(0.0f, 0.0f, 0);
        PowerManagerDefault = new PowerManagerPrototype(0, 0);


        Wall = new TowerPrototype("Wall Tower", 2);

        // Rock Launcher
        RockLauncher[0] = new TowerPrototype("Rock Launcher 1",  10);
        RockLauncher[1] = new TowerPrototype("Rock Launcher 2",  30);
        RockLauncher[2] = new TowerPrototype("Rock Launcher 3",  70);
        RockLauncher[3] = new TowerPrototype("Rock Launcher 4", 150);
        RockLauncher[4] = new TowerPrototype("Rock Launcher 5", 310);
        RockLauncher[5] = new TowerPrototype("Rock Launcher 6", 630);

        RockLauncher[0].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f,   31);
        RockLauncher[1].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f,   96);
        RockLauncher[2].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f,  224);
        RockLauncher[3].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f,  448);
        RockLauncher[4].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f,  960);
        RockLauncher[5].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f, 1680);

        RockLauncher[0].PowerManagerPrototype = new PowerManagerPrototype(50, 10);
        RockLauncher[1].PowerManagerPrototype = new PowerManagerPrototype(150, 30);
        RockLauncher[2].PowerManagerPrototype = new PowerManagerPrototype(350, 70);
        RockLauncher[3].PowerManagerPrototype = new PowerManagerPrototype(750, 150);
        RockLauncher[4].PowerManagerPrototype = new PowerManagerPrototype(1550, 310);
        RockLauncher[5].PowerManagerPrototype = new PowerManagerPrototype(3150, 630);

        Wall.SetUpgradesTo(RockLauncher[0]);

        RockLauncher[0].SetUpgradesTo(RockLauncher[1]);
        RockLauncher[1].SetUpgradesTo(RockLauncher[2]);
        RockLauncher[2].SetUpgradesTo(RockLauncher[3]);
        RockLauncher[3].SetUpgradesTo(RockLauncher[4]);
        RockLauncher[4].SetUpgradesTo(RockLauncher[5]);


        Generator[0] = new TowerPrototype("Generator 1",   5);
        Generator[1] = new TowerPrototype("Generator 2",  15);
        Generator[2] = new TowerPrototype("Generator 3",  35);
        Generator[3] = new TowerPrototype("Generator 4",  75);
        Generator[4] = new TowerPrototype("Generator 5", 155);
        Generator[5] = new TowerPrototype("Generator 6", 315);

        Generator[0].PowerManagerPrototype = new PowerManagerPrototype(  250,  10);
        Generator[1].PowerManagerPrototype = new PowerManagerPrototype(  750,  30);
        Generator[2].PowerManagerPrototype = new PowerManagerPrototype( 1750,  70);
        Generator[3].PowerManagerPrototype = new PowerManagerPrototype( 3750, 150);
        Generator[4].PowerManagerPrototype = new PowerManagerPrototype( 7750, 310);
        Generator[5].PowerManagerPrototype = new PowerManagerPrototype(15750, 630);

        Generator[0].PowerManagerPrototype.PassiveProduction =   5;
        Generator[1].PowerManagerPrototype.PassiveProduction =  15;
        Generator[2].PowerManagerPrototype.PassiveProduction =  35;
        Generator[3].PowerManagerPrototype.PassiveProduction =  75;
        Generator[4].PowerManagerPrototype.PassiveProduction = 155;
        Generator[5].PowerManagerPrototype.PassiveProduction = 315;

        Generator[0].SetUpgradesTo(Generator[1]);
        Generator[1].SetUpgradesTo(Generator[2]);
        Generator[2].SetUpgradesTo(Generator[3]);
        Generator[3].SetUpgradesTo(Generator[4]);
        Generator[4].SetUpgradesTo(Generator[5]);


        //Wall.SetUpgradesTo(Generator[0]);



        // PrototypeSet<TowerPrototype> rockLauncherTower = new PrototypeSet<TowerPrototype>;
        // rockLauncherTower.add(rockLaucnher1); etc
        // PrototypeSet<AttackPrototype> rockLauncherAttack = new PrototypeSet<TowerPrototype>;
        // rockLauncherTower.Attack = rockLauncherAttack;
        // Or something like that. 


        //rockLauncher1.Attack.Projectile = new ProjectilePrototype();
        //rockLauncher1.Attack.Projectile.Payload = new PayloadPrototype();


        /*        
		Rock	Level	Gold	Range	Dmg	Cool	Adj Cool	Energy Cap	Transfer Rate			Targets	Power Cost	Power/Shot		Power	DPS	Pow DPS	Adj Price	Dmg/Gold	Ajd Dmg/Gold							
					1	10		1000	31	3		0.83		50.00		10.00					1.100	25		4.17		5.00	11.37	41.08	15.00	1.14	2.74		Rock Launcher is best at level 3. Unpowered, effectiveness drops to 30-50% of normal, depending on level. Splash helps a bit.					

		// Range is 1/200th of Wc3 values. Really short ranged towers are only 1/100th.
					
		// TowerPrototype(Name, Price)
	
		RockLauncher[0] = new TowerPrototype("Rock Launcher 1", 10);

		// EnergyManagerPrototype(EnergyCap, TransferRate, ConsumptionRate) (Consumption rate is dynamically generated from ability.)
		
		RockLauncher[0].EnergyManager = new EnergyManagerPrototype(50,10,5);
		
		// AttackPrototype(Range, Cooldown);
		
		RockLauncher[0].Attack = new AttackPrototype(5f, 3f);
		
		// ProjectilePrototype(Speed); 
		
		RockLauncher[0].Attack.Projectile = new ProjectilePrototype(8f);
		
		// SplashPayloadPrototype(Damage, )
		
		*/


    }

    // Update is called once per frame
    void Update()
    {

    }
}
