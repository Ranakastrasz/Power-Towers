using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeDatabase : MonoBehaviour
{
    public Sprite WallSprite = null;
	public Sprite CannonSprite = null;
	public Sprite LightningSprite = null;
	public Sprite FrostSprite = null;
	public Sprite WaveSprite = null;
	public Sprite FlameSprite = null;
	public Sprite PosionSprite = null;
	public Sprite GeneratorSprite = null;
	public Sprite TransferSprite = null;

    public static PrototypeDatabase Active { protected set; get; }
    public TowerPrototype Wall;
    public TowerPrototype[] Cannon = new TowerPrototype[6]; // Rock Launcher
    public TowerPrototype[] Lightning = new TowerPrototype[6]; // Tesla Coil
    public TowerPrototype[] Frost = new TowerPrototype[6]; // Lich Tower
    public TowerPrototype[] Wave = new TowerPrototype[6]; // Holy Tower
    public TowerPrototype[] Flame = new TowerPrototype[6]; // Demon Tower
    public TowerPrototype[] Poison = new TowerPrototype[6]; // Chemical Tower
    public TowerPrototype[] Generator = new TowerPrototype[6]; // Waterwheel/Furnace
    public TowerPrototype[] Transfer = new TowerPrototype[6]; // Briding Tower

	public Effect EffectDefault;
	public AttackManagerPrototype AttackManagerDefault;
	public AbilityManagerPrototype AbilityManagerDefault;
    public PowerManagerPrototype PowerManagerDefault;

    private float LevelScale(int level, float scale)
    {
        return ((Mathf.Pow(2, level)) - 1) * scale;
    }

    // Use this for initialization
    void Start()
    {
        Active = this;

		EffectDefault = new Effect();
		AttackManagerDefault = new AttackManagerPrototype(0.0f, 0.0f, 0, EffectDefault);
		AbilityManagerDefault = new AbilityManagerPrototype(0.0f, 0, AbilityManagerPrototype.TRIGGER.ON_ATTACK, 0, EffectDefault);
        PowerManagerDefault = new PowerManagerPrototype(0, 0);


        Wall = new TowerPrototype("Wall Tower", 2, null);

        // Rock Launcher
		{
			int[] Damage = { 31, 96, 224, 448, 960, 1680 }; // Would prefer using a function to generate.
			int BasePrice = 10;
			int BasePower = 50;
			int BasePowerRate = 10;
			for (int z = 0; z < 6; z++)
			{
				// Create a Effectdamage, and a projectile to deliver it.
				ProjectilePrototype projectilePrototype = new ProjectilePrototype (9.0f, new EffectDamage (Damage [z], true));

				// Make an EffectProjectile to throw said projectile
				Effect effect = new EffectProjectile (projectilePrototype, false);
				int level = z + 1;
				// Make a TowerPrototype 
				Cannon [z] = new TowerPrototype ("Rock Launcher " + (z + 1), (int)LevelScale (level, BasePrice), CannonSprite);
				// Attach an AttackManagerPrototype to throw that effect, also display the damage.
				Cannon [z].AttackManagerPrototype = new AttackManagerPrototype (5.0f, 3.0f, Damage [z], effect);
            
				// Attack a SpellManager.
				//Cannon[z].AbilityManagerPrototype = new AbilityManagerPrototype(0.75f,(int)LevelScale(level,5),AbilityManagerPrototype.TRIGGER.CONSTANT,0,effect);
				// (float iCooldown, int iEnergyCost, TRIGGER iTrigger, int iDamageDisplay, Effect iEffect)
				// (5.0f, 25, TRIGGER.ON_ATTACK, -1, new EFFECT(Apply Buff self -cooldown, 5 second, something),"Overclock");


				// Attach a PowerManagerPrototype as well, to supply power.
				Cannon [z].PowerManagerPrototype = new PowerManagerPrototype ((int)LevelScale (level, BasePower), (int)LevelScale (level, BasePowerRate));
			}
    
			for (int z = 0; z < 5; z++)
			{
				Cannon [z].SetUpgradesTo (Cannon [z + 1]);
			}
		}
		{
			// Poison Tower
			int[] Damage = 		  { 20, 53, 107, 196, 341, 567};
			int[] AbilityDamage = { 33, 156, 489, 1306, 3218, 7560 };
			int BasePrice = 20;
			int[] Power = { 150, 400, 900, 1700, 3300, 6400 };
			int[] PowerCost = { 40,113,248,500,979,1890 };
			int[] PowerRate = { 28, 75, 165, 333, 653, 1260 };
			for (int z = 0; z < 6; z++)
			{
				// Create a Effectdamage, and a projectile to deliver it.
				ProjectilePrototype projectilePrototype = new ProjectilePrototype (5.0f, new EffectDamage(Damage[z],true));

				BuffPrototype buff = new BuffPrototype(new EffectDamage(AbilityDamage[z],false),EffectDefault,new EffectDamage(AbilityDamage[z]/10,false),15.1f,1.0f);
				ProjectilePrototype abilityProjectilePrototype = new ProjectilePrototype (9.0f, new EffectBuff(buff,EffectBuff.TARGET.RUNNER,true));

				// Make an EffectProjectile to throw said projectile
				Effect effect = new EffectProjectile (projectilePrototype, false);
				Effect effectAbility = new EffectProjectile (abilityProjectilePrototype, false);
				int level = z + 1;
				// Make a TowerPrototype 
				Poison[z] = new TowerPrototype ("Chemical Tower " + (z + 1), (int)LevelScale (level, BasePrice), CannonSprite);

				// Attach an AttackManagerPrototype to throw that effect, also display the damage.
				Poison[z].AttackManagerPrototype = new AttackManagerPrototype (3.5f, 0.4f, Damage [z], effect);


				// Attack a SpellManager.
				Poison[z].AbilityManagerPrototype = new AbilityManagerPrototype(3.0f,PowerCost[z],AbilityManagerPrototype.TRIGGER.ON_ATTACK,(int)(Damage[z]*2.5f),effectAbility);



				// Attach a PowerManagerPrototype as well, to supply power.
					Poison [z].PowerManagerPrototype = new PowerManagerPrototype ((int)LevelScale (level, Power[z]), (int)LevelScale (level, PowerRate[z]));
			}

			for (int z = 0; z < 5; z++)
			{
					Poison [z].SetUpgradesTo (Poison [z + 1]);
			}
		}


        /*Cannon[0] = new TowerPrototype("Rock Launcher 1",  10);
        Cannon[1] = new TowerPrototype("Rock Launcher 2",  30);
        Cannon[2] = new TowerPrototype("Rock Launcher 3",  70);
        Cannon[3] = new TowerPrototype("Rock Launcher 4", 150);
        Cannon[4] = new TowerPrototype("Rock Launcher 5", 310);
        Cannon[5] = new TowerPrototype("Rock Launcher 6", 630);
        
        Cannon[0].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f,  31, new EffectDamage(  31));
        Cannon[1].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f,  96, new EffectDamage(  96));
        Cannon[2].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f, 224, new EffectDamage( 224));
        Cannon[3].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f, 448, new EffectDamage( 448));
        Cannon[4].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f, 960, new EffectDamage( 960));
        Cannon[5].AttackManagerPrototype = new AttackManagerPrototype(5.0f, 3.0f,1680, new EffectDamage(1680));

        Cannon[0].PowerManagerPrototype = new PowerManagerPrototype(  50,  10);
        Cannon[1].PowerManagerPrototype = new PowerManagerPrototype( 150,  30);
        Cannon[2].PowerManagerPrototype = new PowerManagerPrototype( 350,  70);
        Cannon[3].PowerManagerPrototype = new PowerManagerPrototype( 750, 150);
        Cannon[4].PowerManagerPrototype = new PowerManagerPrototype(1550, 310);
        Cannon[5].PowerManagerPrototype = new PowerManagerPrototype(3150, 630);

        
        Cannon[0].SetUpgradesTo(Cannon[1]);
        Cannon[1].SetUpgradesTo(Cannon[2]);
        Cannon[2].SetUpgradesTo(Cannon[3]);
        Cannon[3].SetUpgradesTo(Cannon[4]);
        Cannon[4].SetUpgradesTo(Cannon[5]);*/


        Generator[0] = new TowerPrototype("Generator 1",   5, GeneratorSprite);
        Generator[1] = new TowerPrototype("Generator 2",  15, GeneratorSprite);
        Generator[2] = new TowerPrototype("Generator 3",  35, GeneratorSprite);
        Generator[3] = new TowerPrototype("Generator 4",  75, GeneratorSprite);
        Generator[4] = new TowerPrototype("Generator 5", 155, GeneratorSprite);
        Generator[5] = new TowerPrototype("Generator 6", 315, GeneratorSprite);
        
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

        
        Transfer[0] = new TowerPrototype("Transfer Tower 1",   3, TransferSprite);
        Transfer[1] = new TowerPrototype("Transfer Tower 2",   9, TransferSprite);
        Transfer[2] = new TowerPrototype("Transfer Tower 3",  21, TransferSprite);
        Transfer[3] = new TowerPrototype("Transfer Tower 4",  45, TransferSprite);
        Transfer[4] = new TowerPrototype("Transfer Tower 5",  93, TransferSprite);
        Transfer[5] = new TowerPrototype("Transfer Tower 6", 189, TransferSprite);
        
        Transfer[0].PowerManagerPrototype = new PowerManagerPrototype(  300,   30);
        Transfer[1].PowerManagerPrototype = new PowerManagerPrototype(  900,   90);
        Transfer[2].PowerManagerPrototype = new PowerManagerPrototype( 2700,  270);
        Transfer[3].PowerManagerPrototype = new PowerManagerPrototype( 8100,  810);
        Transfer[4].PowerManagerPrototype = new PowerManagerPrototype(24300, 2430);
        Transfer[5].PowerManagerPrototype = new PowerManagerPrototype(72900, 7290);
        
        Transfer[0].SetUpgradesTo(Transfer[1]);
        Transfer[1].SetUpgradesTo(Transfer[2]);
        Transfer[2].SetUpgradesTo(Transfer[3]);
        Transfer[3].SetUpgradesTo(Transfer[4]);
        Transfer[4].SetUpgradesTo(Transfer[5]);

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
