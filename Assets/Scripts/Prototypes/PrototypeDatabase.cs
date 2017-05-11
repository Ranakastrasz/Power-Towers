using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrototypeDatabase : MonoBehaviour
{
	public Sprite SpriteBase 			= null;

	public Sprite SpriteGenerator 	    = null;
	public Sprite SpriteTransfer 		= null;


	public Sprite SpriteTurretCannon 		= null;
	public Sprite SpriteTurretLightning 	= null;
	public Sprite SpriteTurretFrost 		= null;
	public Sprite SpriteTurretWave 			= null;
	public Sprite SpriteTurretFlame 		= null;
	public Sprite SpriteTurretPoison	 	= null;


	public Sprite SpriteProjectileCannon 		= null;
	public Sprite SpriteProjectileLightning 	= null;
	public Sprite SpriteProjectileFrost 		= null;
	public Sprite SpriteProjectileWave 			= null;
	public Sprite SpriteProjectileFlame 		= null;
	public Sprite SpriteProjectilePoison	 	= null;


	public Sprite SpriteAbilityProjectileCannon 	= null;
	public Sprite SpriteAbilityProjectileLightning 	= null;
	public Sprite SpriteAbilityProjectileFrost 		= null;
	public Sprite SpriteAbilityProjectileWave 		= null;
	public Sprite SpriteAbilityProjectileFlame 		= null;
	public Sprite SpriteAbilityProjectilePoison	 	= null;



    public static PrototypeDatabase Active { protected set; get; }
    public TowerPrototype Wall;
    public TowerPrototype[] Cannon    	= new TowerPrototype[6]; // Rock Launcher
    public TowerPrototype[] Lightning 	= new TowerPrototype[6]; // Tesla Coil
    public TowerPrototype[] Frost 		= new TowerPrototype[6]; // Lich Tower
    public TowerPrototype[] Wave 		= new TowerPrototype[6]; // Holy Tower
    public TowerPrototype[] Flame 		= new TowerPrototype[6]; // Demon Tower
    public TowerPrototype[] Poison 		= new TowerPrototype[6]; // Chemical Tower
    public TowerPrototype[] Generator 	= new TowerPrototype[6]; // Waterwheel/Furnace
    public TowerPrototype[] Transfer 	= new TowerPrototype[6]; // Briding Tower

	public Effect EffectDefault;
	public AttackManagerPrototype AttackManagerDefault;
	public AbilityManagerPrototype AbilityManagerDefault;
    public PowerManagerPrototype PowerManagerDefault;


	// Replace with a Polynom3 call if feasible.
	// See how many can fit into a Polynom3, for that matter.
	private float LevelScale(int level, float scale)
	{
		return ((Mathf.Pow(2, level)) - 1) * scale;
	}

	private int LevelScale(int level, int scale)
	{
		return (((int)Mathf.Pow(2, level)) - 1) * scale;
	}

    // Use this for initialization
    void Start()
    {
        Active = this;

		EffectDefault = new Effect();
		AttackManagerDefault = new AttackManagerPrototype(0.0f, 0.0f, 0, EffectDefault);
		AbilityManagerDefault = new AbilityManagerPrototype(0.0f, 0, AbilityManagerPrototype.TRIGGER.ON_ATTACK, EffectDefault);
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
				ProjectilePrototype projectilePrototype = new ProjectilePrototype (9.0f, new EffectDamage (Damage [z], true),SpriteProjectileCannon);

				// Make an EffectProjectile to throw said projectile
				Effect effectCreateProjectile = new EffectProjectile (projectilePrototype, false);
				int level = z + 1;
				// Make a TowerPrototype 
				Cannon [z] = new TowerPrototype ("Rock Launcher " + level, (int)LevelScale (level, BasePrice), SpriteTurretCannon);
				// Attach an AttackManagerPrototype to throw that effect, also display the damage.
				Cannon [z].AttackManagerPrototype = new AttackManagerPrototype (5.0f, 3.0f, Damage [z], effectCreateProjectile);

                // Attack a SpellManager.
                Effect effectOverclockAdd = new EffectTowerSpeed("overclock", 4.0f, true);
                Effect effectOverclockRemove = new EffectTowerSpeed("overclock", 0f, false);
				Effect effectAddbuff = new EffectBuff(new BuffPrototype(effectOverclockAdd, effectOverclockRemove, EffectDefault, 5.0f, 0f,"Overclock"),EffectBuff.TARGET.SELF,false);

				Cannon[z].AbilityManagerPrototype = new AbilityManagerPrototype(5.0f,(int)LevelScale(level,25),AbilityManagerPrototype.TRIGGER.CONSTANT,effectAddbuff,"Overclock","+300% Attack Speed");
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
				ProjectilePrototype projectilePrototype = new ProjectilePrototype (5.0f, new EffectDamage(Damage[z],true),SpriteProjectilePoison);

				BuffPrototype buff = new BuffPrototype(new EffectDamage(AbilityDamage[z],false),EffectDefault,new EffectDamage(AbilityDamage[z]/10,false),15.1f,1.0f,"Poison");
				ProjectilePrototype abilityProjectilePrototype = new ProjectilePrototype (9.0f, new EffectBuff(buff,EffectBuff.TARGET.RUNNER,true),SpriteAbilityProjectilePoison);

				// Make an EffectProjectile to throw said projectile
				Effect effect = new EffectProjectile (projectilePrototype, false);
				Effect effectAbility = new EffectProjectile (abilityProjectilePrototype, false);
				int level = z + 1;
				// Make a TowerPrototype 
				Poison[z] = new TowerPrototype ("Posion Tower " + level, (int)LevelScale (level, BasePrice), SpriteTurretPoison);

				// Attach an AttackManagerPrototype to throw that effect, also display the damage.
				Poison[z].AttackManagerPrototype = new AttackManagerPrototype (3.5f, 0.4f, Damage [z], effect);


				// Attack a SpellManager.
				Poison[z].AbilityManagerPrototype = new AbilityManagerPrototype(
					3.0f,PowerCost[z],
					AbilityManagerPrototype.TRIGGER.ON_ATTACK,
					effectAbility,
					"Poison",
					AbilityDamage[z]+"damage + "+(AbilityDamage[z]/10)+ "dps for 15sec"
				);



				// Attach a PowerManagerPrototype as well, to supply power.
					Poison [z].PowerManagerPrototype = new PowerManagerPrototype (Power[z], LevelScale (level, PowerRate[z]));
			}

			for (int z = 0; z < 5; z++)
			{
					Poison [z].SetUpgradesTo (Poison [z + 1]);
			}
		}

		{
			// Flame Tower
			int[] Damage = 		  { 36, 108, 252,  540, 1116, 2268 };
			int[] AbilityDamage = { 48, 135, 294,  585, 1116, 2079 };
			int BasePrice = 20;
			int[] Power = 	  { 120,300, 720,1440,3000,6600 };
			int BasePowerCost = 15;
			int BasePowerRate = 40;
			for (int z = 0; z < 6; z++)
			{
				// Create a Effectdamage, and a projectile to deliver it.
				ProjectilePrototype projectilePrototype = new ProjectilePrototype 		 (5.0f, new EffectDamage(Damage[z]		 			 ,true),SpriteProjectileFlame);
				ProjectilePrototype abilityProjectilePrototype = new ProjectilePrototype (5.0f, new EffectDamage(Damage[z] + AbilityDamage[z],true),SpriteAbilityProjectileFlame);



				// Make an EffectProjectile to throw said projectile
				Effect effect 		 = new EffectProjectile (	    projectilePrototype, false);
				Effect effectAbility = new EffectProjectile (abilityProjectilePrototype, false);
				int level = z + 1;
				// Make a TowerPrototype 
				Flame[z] = new TowerPrototype ("Flame Tower " + level, (int)LevelScale (level, BasePrice), SpriteTurretFlame);

				// Attach an AttackManagerPrototype to throw that effect, also display the damage.
				Flame[z].AttackManagerPrototype = new AttackManagerPrototype (3.0f, 0.75f, Damage [z], effect);


				// Attack a SpellManager.
				Flame[z].AbilityManagerPrototype = new AbilityManagerPrototype(
					0.75f,(int)LevelScale (level, BasePowerCost),
					AbilityManagerPrototype.TRIGGER.ON_ATTACK_OVERRIDE,
					effectAbility,
					"Ignite",
					AbilityDamage[z]+" bonus damage."
				);



				// Attach a PowerManagerPrototype as well, to supply power.
				Flame [z].PowerManagerPrototype = new PowerManagerPrototype (Power[z], (int)LevelScale (level, BasePowerRate));
			}

			for (int z = 0; z < 5; z++)
			{
				Flame [z].SetUpgradesTo (Flame [z + 1]);
			}
		}

		{
			// Lightning Tower
			int BaseDamage = 3;
			int BaseAbilityDamage = 9;//90;
			int BasePrice = 10;
			int BasePower = 160;
			int BasePowerCost = 2;//20;
			int BasePowerRate = 40;
			for (int z = 0; z < 6; z++)
			{
				int level = z + 1;

				// Create a Effectdamage, and a projectile to deliver it.
				ProjectilePrototype projectilePrototype        = new ProjectilePrototype ( 5.0f, new EffectDamage(LevelScale (level, BaseDamage		  ), true), SpriteProjectileLightning);
				ProjectilePrototype abilityProjectilePrototype = new ProjectilePrototype ( 8.0f, new EffectDamage(LevelScale (level, BaseAbilityDamage), true), SpriteAbilityProjectileLightning);



				// Make an EffectProjectile to throw said projectile
				Effect effect 		 = new EffectProjectile (	    projectilePrototype, false);
				Effect effectAbility = new EffectProjectile (abilityProjectilePrototype, false);
				// Make a TowerPrototype 
				Lightning[z] = new TowerPrototype ("Lightning Tower " + level, LevelScale (level, BasePrice), SpriteTurretLightning);

				// Attach an AttackManagerPrototype to throw that effect, also display the damage.
				Lightning[z].AttackManagerPrototype = new AttackManagerPrototype (3.0f, 1f, LevelScale(level, BaseDamage), effect);


				// Attack a SpellManager.
				Lightning[z].AbilityManagerPrototype = new AbilityManagerPrototype(
					0.1f,LevelScale (level, BasePowerCost),
					AbilityManagerPrototype.TRIGGER.ON_ATTACK_OVERRIDE,
					effectAbility,
					"Lightning",
					LevelScale (level, BaseAbilityDamage)+" damage."
				);



				// Attach a PowerManagerPrototype as well, to supply power.
				Lightning [z].PowerManagerPrototype = new PowerManagerPrototype (LevelScale(level,BasePower), (int)LevelScale (level, BasePowerRate));
			}

			for (int z = 0; z < 5; z++)
			{
				Lightning [z].SetUpgradesTo (Lightning [z + 1]);
			}
		}

		{
			// Generator
			int BasePrice = 5;
			int BasePowerGeneration = 5;
			int BasePowerRate = 10;
			int BasePowerCap = 250;

			for (int z = 0; z < 6; z++)
			{

				int level = z + 1;

				Generator [z] = new TowerPrototype ("Generator " + level, (int)LevelScale (level, BasePrice), SpriteGenerator);

				// Attach a PowerManagerPrototype as well, to supply power.
				Generator [z].PowerManagerPrototype = new PowerManagerPrototype ((int)LevelScale (level, BasePowerCap), (int)LevelScale (level, BasePowerRate));

				Generator[z].PowerManagerPrototype.PassiveProduction =  (int)LevelScale(level,BasePowerGeneration);

			}

			for (int z = 0; z < 5; z++)
			{
				Generator [z].SetUpgradesTo (Generator [z + 1]);
			}
		}

		{
			// Transfer Tower
			int BasePrice = 3;
			int BasePowerRate = 30;
			int BasePowerCap = 300;

			for (int z = 0; z < 6; z++)
			{

				int level = z + 1;

				Transfer [z] = new TowerPrototype ("Transfer Tower " + level, (int)LevelScale (level, BasePrice), SpriteTransfer);

				// Attach a PowerManagerPrototype as well, to supply power.
				Transfer [z].PowerManagerPrototype = new PowerManagerPrototype (((int)Mathf.Pow(3, level-1)*BasePowerCap), ((int)Mathf.Pow(3, level-1)*BasePowerRate));



			}

			for (int z = 0; z < 5; z++)
			{
				Transfer [z].SetUpgradesTo (Transfer [z + 1]);
			}
		}


    }

    // Update is called once per frame
    void Update()
    {

    }
}
