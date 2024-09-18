using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace HardTimeKeybindCheats
{
    [BepInPlugin(PluginGuid, PluginName, PluginVer)]
    [HarmonyPatch]
    public class Plugin : BaseUnityPlugin
    {
        public const string PluginGuid = "IngoH.HardTime.HardTimeKeybindCheats";
        public const string PluginName = "HardTimeKeybindCheats";
        public const string PluginVer = "1.0.3";

        internal static ManualLogSource Log;
        internal readonly static Harmony Harmony = new(PluginGuid);

        internal static string PluginPath;

        enum TargetMode
        {
            TARGET_ALL,
            TARGET_PLAYERS,
            TARGET_NPC,
            TARGET_RANDOM,
            TARGET_RANDOM_PLAYER,
            TARGET_RANDOM_NPC
        }

        private static TargetMode targetMode = TargetMode.TARGET_ALL;
        

        private void Awake()
        {
            Plugin.Log = base.Logger;

            PluginPath = Path.GetDirectoryName(Info.Location);
        }

        private void OnEnable()
        {
            Harmony.PatchAll();
            Logger.LogInfo($"Loaded {PluginName}!");
        }

        private void OnDisable()
        {
            Harmony.UnpatchSelf();
            Logger.LogInfo($"Unloaded {PluginName}!");
        }
        
        private static float _explosiondelay;
        private static float _keyCooldown = 0;
        
        private void Update()
        {
            UpdateTextCheats();
            if (Input.GetKey(KeyCode.E) && Input.GetKey(KeyCode.X) && SceneManager.GetActiveScene().name == "Game" && Time.time > _explosiondelay)
            {
                var size = Random.Range(7f, 10f);
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    size *= 2;
                }
                if (Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt))
                {
                    size *= 4;
                }
                ALIGLHEIAGO.MDFJMAEDJMG(3, 1,  new UnityEngine.Color(1f, Random.Range(0.3f,0.7f), 0f), size, null, Random.Range(-40,40f), Random.Range(-10,10f), Random.Range(-40,40f), 0f, 0f, 0.1f);
                if (!Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
                {
                    _explosiondelay = Time.time + 0.1f;
                }
            }

            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.End) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    if (cd == null) continue;
                    cd.health /= 2;
                    cd.hp = 0;
                    if (MappedGlobals.Rnd(1, 2) == 1)
                    {
                        cd.SellBackFall();
                    }
                    else
                    {
                        cd.SellFrontFall();
                    }
                }
            }
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.I) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    cd.RiskInjury(Random.Range(0, 4), int.MaxValue);
                    if (MappedGlobals.Rnd(1, 2) == 1)
                    {
                        cd.SellBackFall();
                    }
                    else
                    {
                        cd.SellFrontFall();
                    }
                }
            }
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.L) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    var count = 0;
                    int limb;
                    int ex = (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl)) ? 1 : 0;
                    do
                    {
                        limb = MappedGlobals.Rnd(3 + ex, 15);
                        count++;
                    }
                    while (cd.LimbIntact(limb) == 0 && count < 1000);
                    
                    cd.scar[limb] = -300;
			        MappedSound.Emit(cd.audio, MappedSound.bleed[1], 0.8f);
                    cd.Spurt(3, 101, Color.red, Random.Range(6f, 10f), limb, 0f, 0f, 0f, 0f, 0f, 0.1f);
			        cd.Spurt(2, 101, MappedTextures.fleshColour * cd.costume.SkinFilter(), Random.Range(4f, 6f), limb, 0f, 0f, 0f, 0f, 0f, 0.1f);
                    cd.AgonySound(MappedGlobals.Rnd(2, 3));
			        cd.model[limb].SetActive(false);
                    switch (limb)
			        {
                        case 4:
                        case 5:
                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 11:
                        case 12:
                            MappedWeapons.Produce(83, cd.LimbX(limb), cd.LimbY(limb), cd.LimbZ(limb), MappedGlobals.Rnd(0, 359), cd.scale);
                            break;
                    }
                    MappedWeapons.Produce(MappedGlobals.Rnd(88, 91), cd.LimbX(limb), cd.LimbY(limb), cd.LimbZ(limb), MappedGlobals.Rnd(0, 359), cd.scale);
                    cd.injury = limb;
                    cd.health /= 2f;
                    cd.hp = 0f;
                    cd.dt = cd.GetDT(1000f);
                    MappedSound.Pop(cd.id, 0, 1f);
                    cd.spirit /= 2f;
                    if (limb == 3)
                    {
                        cd.health = 0f;
                        cd.spirit = 0f;
                        cd.dead = cd.charID;
                        if (MappedMatch.state > 0 && cd.role == 1 && cd.eliminated == 0f)
                        {
                            MappedMatch.DeclareFall(cd.attacker, cd.id);
                        }
                        cd.eliminated = 1f;
                    }
                    for (int j = 2; j <= 5; j++)
                    {
                        cd.charData.stat[j] = Mathf.Lerp(cd.charData.stat[j], 50f, 0.25f);
                    }
                    if (MappedGlobals.Rnd(1, 2) == 1)
                    {
                        cd.SellBackFall();
                    }
                    else
                    {
                        cd.SellFrontFall();
                    }
                }
            }

            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.H) && SceneManager.GetActiveScene().name == "Game")
            {
                var nurseWhat = 0;
                foreach (MappedPlayer cd in GetTargets())
                {
                    for (int j = 1; j <= 16; j++)
                    {
                        if (cd.scar[j] < 0)
                        {
                            cd.scar[j] = 0;
                            cd.model[j].SetActive(true);
                            nurseWhat = j;
                        }
                        if (cd.charData.scar[j] < 0)
                        {
                            cd.charData.scar[j] = 0;
                        }
                    }
                    if (nurseWhat > 0)
                    {
                        MappedSound.Emit(cd.voice, MappedSound.relief, cd.charData.voice, 0.5f);
                        MappedSound.Emit(cd.audio,  MappedSound.bleed[1], -0.1f, 0.5f);
                        if (cd.CollapseViable() > 0)
                        {
                            cd.NurseLimb(nurseWhat);
                        }
                    }
                    FullHeal(cd);
                }
            }
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha1) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    if (cd == null) continue;
                    cd.health = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ? 0 : 5000 * MappedGlobals.optLength;
                    cd.healthLimit = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ? 0 : 5000 * MappedGlobals.optLength;
                }
            }
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha2) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    if (cd == null) continue;
                    cd.spirit = Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl) ? 0 : 1000;
                }
            }
                    
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha3) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    if (cd == null) continue;
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        cd.toilet = -19;
                        cd.EmptyBowels();
                    }
                    else
                    {
                        cd.toilet = 19;
                        cd.EmptyBowels();
                    }
                }
            }
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha4) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    if (cd == null) continue;
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        cd.toilet = 0;
                    }
                    else
                    {
                        MappedSound.Emit(cd.voice, MappedSound.drown[MappedGlobals.Rnd(1, 3)], cd.charData.voice);
                        MappedSound.Emit(cd.audio, MappedSound.splash, -0.1f, 0.5f);
                        cd.Spurt(3, 101, new UnityEngine.Color(0.9f, Random.Range(0.6f, 0.9f), 0.3f),
                            Random.Range(5f, 8f), 3, 0f, 0f, 0f, cd.a, 0.25f * cd.scale, 0.1f);
                        cd.health /= 2f;
                        cd.spirit /= 2f;
                        cd.blind = -MappedGlobals.Rnd(100, 300);
                        cd.dt = Mathf.Abs(cd.blind);
                        if (cd.anim == 0 || cd.anim == 10)
                        {
                            cd.ChangeAnim(805);
                        }

                        if (cd.id == MappedPlayers.star)
                        {
                            MappedPromo.RiskFeedback(55, 0);
                        }
                    }
                }
            }

            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha5) &&
                SceneManager.GetActiveScene().name == "Game")
            {
                if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                {
                    MappedItems.Add(0);
                }
                else
                {
                    MappedWeapons.Add(0);
                }
                
            }
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha6) &&
                SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedWeapon w in MappedWeapons.weap)
                {
                    if (w == null) continue;
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        w.fireTim = 0;
                        if (w.fire != null)
                        {
                            w.fire.SetActive(false);
                        }
                    }
                    else
                    {
                        w.Ignite();
                    }
                }

                foreach (MappedItem i in MappedItems.item)
                {
                    if (i == null) continue;
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        i.fireTim = 0;
                        if (i.fire != null)
                        {
                            i.fire.SetActive(false);
                        }
                    }
                    else
                    {
                        i.Break();
                    }
                }
            }
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha7) &&
                SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedItem i in MappedItems.item)
                {
                    if (i == null) continue;
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        if (i.state < 0)
                        {
                            if (i.anim == 22)
                            {
                                i.anim = 21;
                            }
                            else
                            {
                                i.anim = 20;
                            }

                            i.animTim = 0f;
                            i.state = 1;
                        }
                    }
                    else {
                        if (i.state >= 0)
                        {
                            i.Break();
                        }
                    }
                }
            }

            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha8) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    if (cd == null) continue;
                    if (Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
                    {
                        cd.charData.warrant = 0;
                    }
                    else {
                        cd.charData.warrant = MappedGlobals.Rnd(1, 29);
                        ApplyWarrantVars(cd);
                    }
                }
            }

        
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha9) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    if (cd == null) continue;
                    cd.charData.warrant = -MappedGlobals.Rnd(1, 29);
                    ApplyWarrantVars(cd);
                }
            }
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Alpha0) && SceneManager.GetActiveScene().name == "Game")
            {
                foreach (MappedPlayer cd in GetTargets())
                {
                    if (cd == null) continue;
                    var deadChars = MappedCharacters.c.Where(c => c.dead > 0).ToList();
                    var notDeadChars = MappedCharacters.c.Where(c => c.dead == 0 && c.id != cd.charData.id).ToList();
                    if (deadChars.Count > 0)
                    {
                        cd.birthSoul = Array.IndexOf(MappedCharacters.c, deadChars[Random.Range(0, deadChars.Count)]);
                        cd.charData.pregnant = Array.IndexOf(MappedCharacters.c, notDeadChars[Random.Range(0, notDeadChars.Count)]);
                        if (cd.anim == 0 || cd.anim == 10 || cd.anim == 25 || cd.anim == 996)
                        {
                            cd.ChangeAnim(948, -28f);
                        }
                        else
                        {
                            cd.ChangeAnim(948, -8f);
                        }
                    }
                }
            }

            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Comma) &&
                SceneManager.GetActiveScene().name == "Game")
            {
                targetMode = (TargetMode) (((int) targetMode + Enum.GetNames(typeof(TargetMode)).Length - 1) % Enum.GetNames(typeof(TargetMode)).Length);
                MappedMatch.PostComment($"Target mode: {targetMode.ToString().Replace("TARGET_", "").Replace("NON_", "NON-").Replace("_", " ")}");
            }
            
            if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetKeyDown(KeyCode.Period) &&
                SceneManager.GetActiveScene().name == "Game")
            {
                targetMode = (TargetMode) (((int) targetMode + 1) % Enum.GetNames(typeof(TargetMode)).Length);
                MappedMatch.PostComment($"Target mode: {targetMode.ToString().Replace("TARGET_", "").Replace("NON_", "NON-").Replace("_", " ")}");
            }

            if (SceneManager.GetActiveScene().name != "Game") {
                return;
            }

            var pStar = (MappedPlayer) MappedPlayers.p[MappedPlayers.star];      
            if (Input.GetKey(KeyCode.PageUp)) {
                if (pStar.anim != 1401) {
                    pStar.ChangeAnim(1401, 20);
                }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                    pStar.gravity = 1f;
                } else {
                    pStar.gravity = 0.25f;
                }
            }

            if (_keyCooldown > 0) {
                _keyCooldown -= Time.deltaTime;
                return;
            }
             
            if (Input.GetKey(KeyCode.Equals) || Input.GetKey(KeyCode.Plus) || Input.GetKey(KeyCode.KeypadPlus) || Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus))
            {
                var mult = 1;
                if (Input.GetKey(KeyCode.Minus) || Input.GetKey(KeyCode.KeypadMinus)) {
                    mult = -1;
                }
                if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)) {
                    mult *= 10;
                }
                if (Input.GetKey(KeyCode.E)) {
                    pStar.health += 50f * MappedGlobals.optLength * mult;
                    MappedSound.Play(MappedSound.change);
                }
                if (Input.GetKey(KeyCode.H)) {
                    pStar.spirit += 10f * mult;
                    MappedSound.Play(MappedSound.change);
                }
                if (Input.GetKey(KeyCode.R)) {
                    pStar.charData.ChangeStat(1, mult);
                    MappedSound.Play(MappedSound.change);
                }
                if (Input.GetKey(KeyCode.S)) {
                    pStar.charData.ChangeStat(2, mult);
                    MappedSound.Play(MappedSound.change);
                }
                if (Input.GetKey(KeyCode.I)) {
                    pStar.charData.ChangeStat(3, mult);
                    MappedSound.Play(MappedSound.change);
                }
                if (Input.GetKey(KeyCode.A)) {
                    pStar.charData.ChangeStat(4, mult);
                    MappedSound.Play(MappedSound.change);
                }
                if (Input.GetKey(KeyCode.T)) {
                    pStar.charData.ChangeStat(5, mult);
                    MappedSound.Play(MappedSound.change);
                }
                if (Input.GetKey(KeyCode.D)) {
                    pStar.charData.ChangeStat(6, mult);
                    MappedSound.Play(MappedSound.change);
                }
                if (Input.GetKey(KeyCode.M)) {
                    Progress.bank += mult;
                    MappedSound.Play(MappedSound.change);
                }
                if (Input.GetKey(KeyCode.End)) {
                    Progress.sentence += mult;
                    MappedSound.Play(MappedSound.change);
                }
                _keyCooldown = 0.25f;
            }
            
        }

        List<char> charHist = new();

        private void UpdateTextCheats() {
            if (SceneManager.GetActiveScene().name != "Game") return;
            var str = Input.inputString;
            if (str.Length == 0) return;
            foreach (var c in str) {
                charHist.Add(c);
            }
            if (charHist.Count > 100) {
                charHist.RemoveAt(0);
            }
            var copy = charHist.ToArray();
            var cheat = new string(copy).ToLower().Replace(" ", "").Replace("\n", "").Replace("\r", "").Replace("\t", "").Replace("\b", "");
            var pStar = (MappedPlayer) MappedPlayers.p[MappedPlayers.star];
            var anyCheat = true;
            if (cheat.EndsWith("takemehome"))
			{
				Characters.cStar.home = 25;
				Progress.addressStreet = 24;
				Progress.addressDoor = 1;
			}
            else if (cheat.EndsWith("fullofenergy"))
			{
				pStar.health = 5000f;
			}
            else if (cheat.EndsWith("whatarush"))
            {
                pStar.spirit = 900f;
            }
            else if (cheat.EndsWith("bathroombreak"))
			{
				pStar.toilet = 0f;
			}
            else if (cheat.EndsWith("showmethemoney"))
            {
                MappedProgress.bank += 1000; 
            }
            else if (cheat.EndsWith("teflondon"))
            {
                Characters.cStar.warrant = 0;
            }
            else if (cheat.EndsWith("heartbreakkid"))
            {
                for (int i = 1; i <= Characters.no_chars; i++)
				{
					if (Characters.c[i].gender != Characters.cStar.gender && Characters.c[i].HBLGJBAJPBN(Characters.star) > 0)
					{
						((MappedCharacter) Characters.c[i]).ChangeRelationship(Characters.star, 69);
					}
				}
            }
            else if (cheat.EndsWith("trendsetter"))
            {
                Characters.fashion[0] = Characters.KELJBHEHGMO();
				Characters.fashion[1] = Characters.KELJBHEHGMO();
				if (Characters.fashion[0] == Characters.fashion[1])
				{
					Characters.fashion[MappedGlobals.Rnd(0, 1)] = 0;
				}
            }
            else if (cheat.EndsWith("ingoodshape"))
            {
                FullHeal(pStar);
            }
            else if (cheat.EndsWith("inexcellentshape"))
            {
                int nurseWhat = 0;
                for (int j = 1; j <= 16; j++)
                {
                    if (pStar.scar[j] < 0)
                    {
                        pStar.scar[j] = 0;
                        pStar.model[j].SetActive(true);
                        nurseWhat = j;
                    }
                    if (pStar.charData.scar[j] < 0)
                    {
                        pStar.charData.scar[j] = 0;
                    }
                }
                if (nurseWhat > 0)
                {
                    MappedSound.Emit(pStar.voice, MappedSound.relief, pStar.charData.voice, 0.5f);
                    MappedSound.Emit(pStar.audio,  MappedSound.bleed[1], -0.1f, 0.5f);
                    if (pStar.CollapseViable() > 0)
                    {
                        pStar.NurseLimb(nurseWhat);
                    }
                }
                FullHeal(pStar);
            }
            else if (cheat.EndsWith("inexcellentshape"))
            {
                int nurseWhat = 0;
                for (int j = 1; j <= 16; j++)
                {
                    if (pStar.scar[j] < 0)
                    {
                        pStar.scar[j] = 0;
                        pStar.model[j].SetActive(true);
                        nurseWhat = j;
                    }
                    if (pStar.charData.scar[j] < 0)
                    {
                        pStar.charData.scar[j] = 0;
                    }
                }
                if (nurseWhat > 0)
                {
                    MappedSound.Emit(pStar.voice, MappedSound.relief, pStar.charData.voice, 0.5f);
                    MappedSound.Emit(pStar.audio,  MappedSound.bleed[1], -0.1f, 0.5f);
                    if (pStar.CollapseViable() > 0)
                    {
                        pStar.NurseLimb(nurseWhat);
                    }
                }
                FullHeal(pStar);
            }
            else if (cheat.EndsWith("deadoralive"))
            {
                Characters.cStar.warrant = MappedGlobals.Rnd(1, 29);
                ApplyWarrantVars(pStar);
            }
            else if (cheat.EndsWith("takemetocourt"))
            {
                Characters.cStar.warrant = MappedGlobals.Rnd(1, 29);
                ApplyWarrantVars(pStar);
                MappedPromo.meeting = 800 + Characters.cStar.warrant;
				MappedPromo.variable = Characters.cStar.warrantVariable;
                int otherChar;
                var c = 0;
                do {
                    otherChar = MappedGlobals.Rnd(1, Characters.no_chars);
                    c++;
                } while ((otherChar == Characters.star || otherChar == Characters.cStar.warrantVictim || otherChar == Characters.cStar.warrantWitness) && c < 1000);
				UnmappedPromo.PLNGAHCNLNM = otherChar;
				UnmappedPromo.FJKBIOHJIAI = Characters.star;
				MappedPromo.location = 20;
				MappedWorld.Exit(70);
				MappedPromo.script = 0;
            }
            else if (cheat.EndsWith("civilianlife"))
            {
                pStar.charData.ChangeRole(0); // Civilian
            }
            else if (cheat.EndsWith("gotojail"))
            {
                pStar.charData.ChangeRole(1); // Prisoner
            }
            else if (cheat.EndsWith("iamthelaw"))
            {
                pStar.charData.ChangeRole(3); // Officer
            }
            else if (cheat.EndsWith("notalongtime"))
            {
                Progress.sentence = 0;
            }
            else if (cheat.EndsWith("lifesentence"))
            {
                Progress.sentence = 36525;
            }
            else if (cheat.EndsWith("toughlove"))
            {
                for (int i = 1; i <= Characters.no_chars; i++)
				{
					((MappedCharacter) Characters.c[i]).ChangeRelationship(Characters.star, -1);
				}
            }
            else if (cheat.EndsWith("silenttreatment"))
            {
                for (int i = 1; i <= Characters.no_chars; i++)
				{
					((MappedCharacter) Characters.c[i]).ChangeRelationship(Characters.star, 0);
				}
            }
            else if (cheat.EndsWith("buddysystem"))
            {
                for (int i = 1; i <= Characters.no_chars; i++)
				{
					((MappedCharacter) Characters.c[i]).ChangeRelationship(Characters.star, 1);
				}
            }
            else if (cheat.EndsWith("unlimitedpower"))
            {
                for (int i = 1; i <= 6; i++)
                {
                    pStar.charData.ChangeStat(i, 9999);
                }
            } else {
                anyCheat = false;
            }
            if (anyCheat) {
                charHist.Clear();
            }
        }

        private List<MappedPlayer> GetTargets()
        {
            switch (targetMode)
            {
                case TargetMode.TARGET_ALL:
                    return MappedPlayers.p.Skip(1).Select(p => (MappedPlayer) p).ToList();
                case TargetMode.TARGET_PLAYERS:
                    return MappedPlayers.p.Skip(1).Select(p => (MappedPlayer)p).Where(p => p != null && p.control >= 0).ToList();
                case TargetMode.TARGET_NPC:
                    return MappedPlayers.p.Skip(1).Select(p => (MappedPlayer)p).Where(p => p != null && p.control < 0).ToList();
                case TargetMode.TARGET_RANDOM:
                    return MappedPlayers.p.Skip(1).Select(p => (MappedPlayer)p).OrderBy(c => Guid.NewGuid()).Take(1).ToList();
                case TargetMode.TARGET_RANDOM_PLAYER:
                    return MappedPlayers.p.Skip(1).Select(p => (MappedPlayer)p).Where(p => p != null && p.control >= 0).OrderBy(c => Guid.NewGuid()).Take(1).ToList();
                case TargetMode.TARGET_RANDOM_NPC:
                    return MappedPlayers.p.Skip(1).Select(p => (MappedPlayer)p).Where(p => p != null && p.control < 0).OrderBy(c => Guid.NewGuid()).Take(1).ToList();
                default:
                    return MappedPlayers.p.Skip(1).Select(p => (MappedPlayer)p).ToList();
            }
        }

        private void ApplyWarrantVars(MappedPlayer cd)
        {
            cd.charData.warrantVariable = 0;
            cd.charData.warrantVictim = 0;
            cd.charData.warrantWitness = 0;
            switch (Math.Abs(cd.charData.warrant))
            {
                case 3:
                    // Possession of <item>
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 103);
                    if (cd.charData.warrantVariable == 103)
                    {
                        cd.charData.warrantVariable = -1;
                    }
                    break;
                case 4:
                    // Vandalism of <furniture> or <location>
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 81);
                    if (cd.charData.warrantVariable == 81)
                    {
                        cd.charData.warrantVariable = -9999;
                    } else if (cd.charData.warrantVariable > 36)
                    {
                        cd.charData.warrantVariable = -cd.charData.warrantVariable + 36;
                    }
                    break;
                case 6:
                    // Theft of <item>
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 103);
                    if (cd.charData.warrantVariable == 103)
                    {
                        cd.charData.warrantVariable = -1;
                    }
                    break;
                case 7:
                    // Fornication with <person>
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, MappedCharacters.no_chars);
                    break;
                case 8:
                    // Arson of <item> or <furniture>
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 139);
                    if (cd.charData.warrantVariable == 139)
                    {
                        cd.charData.warrantVariable = -1;
                    } else if (cd.charData.warrantVariable > 102)
                    {
                        cd.charData.warrantVariable = -cd.charData.warrantVariable + 102;
                    }
                    break;
                case 10:
                    // Assault with <item>
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 103);
                    if (cd.charData.warrantVariable == 103)
                    {
                        cd.charData.warrantVariable = -1;
                    }
                    break;
                case 17:
                    // Trading <item>
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 103);
                    if (cd.charData.warrantVariable == 103)
                    {
                        cd.charData.warrantVariable = -1;
                    }
                    break;
                case 19:
                    // Urination or defecation at <location>
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 2);
                    if (cd.charData.warrantVariable == 2)
                    {
                        cd.charData.warrantVariable = -1;
                    }
                    cd.charData.warrantVariable *= MappedGlobals.Rnd(1, 54);
                    break;
                case 25:
                    // Snacking on <item>
                    // Yes, picking a random item includes non-foods, but that just makes it funnier
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 103);
                    if (cd.charData.warrantVariable == 103)
                    {
                        cd.charData.warrantVariable = -1;
                    }
                    break;
                case 26:
                    // Driving <item> or <furniture> recklessly
                    // Also includes non-vehicles, but that's just funny
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 139);
                    if (cd.charData.warrantVariable == 139)
                    {
                        cd.charData.warrantVariable = -1;
                    } else if (cd.charData.warrantVariable > 36)
                    {
                        cd.charData.warrantVariable = -cd.charData.warrantVariable + 36;
                    }
                    break;
                case 27:
                    // Discharging <item>
                    // Also includes non-weapons, but that's just funny
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 103);
                    if (cd.charData.warrantVariable == 103)
                    {
                        cd.charData.warrantVariable = -1;
                    }
                    break;
                case 28:
                    // Animal abuse of <animal>
                    cd.charData.warrantVariable = MappedGlobals.Rnd(1, 2);
                    break;
                default:
                    break;
            }
            var c = 0;
            do {
                cd.charData.warrantVictim = MappedGlobals.Rnd(1, Characters.no_chars);
                c++;
            } while (cd.charData.warrantVictim == cd.charData.id && c < 1000);
            if (MappedGlobals.Rnd(1, 2) == 1)
            {
                c = 0;
                do {
                    cd.charData.warrantWitness = MappedGlobals.Rnd(1, Characters.no_chars);
                    c++;
                } while ((cd.charData.warrantWitness == cd.charData.id || cd.charData.warrantVictim == cd.charData.warrantWitness) && c < 1000);
            }
        }

        private void FullHeal(MappedPlayer cd)
        {
            cd.charData.injury = 0;
            cd.injury = 0;
            cd.health = 5000f * MappedGlobals.optLength;
            cd.healthLimit = 5000f * MappedGlobals.optLength;
            cd.hp = 1000f;
            cd.toilet = 0;
            cd.dt = 0;
            cd.blind = 0;
            cd.breath = 100f;
            if (cd.spirit < 500f) {
                cd.spirit = 500f;
            }
            for (int j = 1; j <= 16; j++)
            {
                if (cd.scar[j] > 0) {
                    cd.scar[j] = 0;
                }
            }
        }
    }
}