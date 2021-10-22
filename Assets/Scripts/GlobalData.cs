using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

[InitializeOnLoad]
public static class GlobalData
{
    public static int numToolSlots = new int();
    public static int[] levelUpExp = new int[50];

    public static IDictionary<string, string[]> Recipes = new Dictionary<string, string[]>();

    public static Dictionary<string, Dictionary<string, string>> itemDict;
    public static Dictionary<string, Dictionary<string, string>> objectsDict;


    static GlobalData()
    {
        // Misc game data
        numToolSlots = 12;

        for (int i = 0; i < levelUpExp.Length; i++)
        {
            if (i < 10) { levelUpExp[i] = (int)Math.Ceiling((100) * Math.Pow((i + 1), 1.3)); }
            else if (i < 20) { levelUpExp[i] = (int)Math.Ceiling((100) * Math.Pow((i + 1), 1.5)); }
            else if (i < 30) { levelUpExp[i] = (int)Math.Ceiling((100) * Math.Pow((i + 1), 1.7)); }
            else if (i < 40) { levelUpExp[i] = (int)Math.Ceiling((100) * Math.Pow((i + 1), 1.9)); }
            else if (i < 50) { levelUpExp[i] = (int)Math.Ceiling((100) * Math.Pow((i + 1), 2.1)); }
        }

        // Recipe data
        Recipes.Add("craftingTable", new string[] { "logsNormal" });
        Recipes.Add("furnace", new string[] { "rockNormal" });

        // Item data
        itemDict = new Dictionary<string, Dictionary<string, string>>() {
            {"axeNormal",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "axeNormal"}, {"toolType", "axe"}, {"displayName", "axe"}, {"shopCost", "10"},
                                                           {"exp", "0"}, {"itemDescription", "A sturdy blade good for chopping down good ol' wood"}} },
            {"logsNormal",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "logsNormal"}, {"toolType", "logs"}, {"displayName", "logs"}, {"shopCost", "3"},
                                                            {"exp", "25"}, {"itemDescription", "Normal logs, used for all sorts of sturdy projects"}}},
            {"logsBirch",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "logsBirch"}, {"toolType", "logs"}, {"displayName", "birch logs"}, {"shopCost", "5"},
                                                           {"exp", "50"},{"itemDescription", "Add a lean white hue to your crafting project"}}},
            {"shovelNormal",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "shovelNormal"}, {"toolType", "shovel"}, {"displayName", "shovel"}, {"shopCost", "10"},
                                                              {"exp", "0"}, {"itemDescription", "Dig it up oh ohh, dig it"}}},
            {"pickaxeNormal",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "pickaxeNormal" }, {"toolType", "pickaxe"}, {"displayName", "pickaxe"}, {"shopCost", "10"},
                                                               {"exp", "0"}, {"itemDescription", "Those ores aren't going to extract themselves..."}}},
            {"scytheNormal",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "scytheNormal" }, {"toolType", "scythe"}, {"displayName", "scythe"}, {"shopCost", "10"},
                                                              {"exp", "0"}, {"itemDescription", "Pick your veggies with one clean swipe"}}},
            {"rockNormal",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "rockNormal" }, {"toolType", "rock"}, {"displayName", "rock"}, {"shopCost", "5"},
                                                            {"exp", "25"}, {"itemDescription", "A gray, inanimate object"}}},
            {"furnace",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "furnace" }, {"toolType", "furnace"}, {"displayName", "furnace"}, {"shopCost", "20"},
                                                         {"exp", "0"}, {"itemDescription", "Kind of like a campfire for rocks"}}},
            {"craftingTable",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "craftingTable" }, {"toolType", "craftingTable"}, {"displayName", "crafting table"}, {"shopCost", "20"},
                                                               {"exp", "0"}, {"itemDescription", "Imagination and a crafting table can get you a long ways"}}},
            {"oreCopper",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "oreCopper" }, {"toolType", "ore"}, {"displayName", "copper ore"}, {"shopCost", "5"},
                                                           {"exp", "50"}, {"itemDescription", "Inexpensive ore with a reddish...erm...copper hue"}}},
            {"oreIron",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "oreIron" }, {"toolType", "ore"}, {"displayName", "iron ore"}, {"shopCost", "10"},
                                                         {"exp", "100"}, {"itemDescription", "Ore for those with a boring flavor of style"}}},
            {"oreGold",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "oreGold" }, {"toolType", "ore"}, {"displayName", "gold ore"}, {"shopCost", "20"},
                                                          {"exp", "200"}, {"itemDescription", "Unpack your pans, we're going West"}}},
            {"orePlatinum",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "orePlatinum" }, {"toolType", "ore"}, {"displayName", "platinum ore"}, {"shopCost", "40"},
                                                             {"exp", "400"}, {"itemDescription", "Shiny, highly sought after ore"}}},
            {"oreGemstone",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "oreGemstone" }, {"toolType", "ore"}, {"displayName", "gemstone"}, {"shopCost", "80"},
                                                             {"exp", "800"}, {"itemDescription", "Ore with a brilliant vibrance"}}},
            {"oreDark",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "oreDark" }, {"toolType", "ore"}, {"displayName", "dark ore"}, {"shopCost", "160"},
                                                         {"exp", "1600"}, {"itemDescription", "Mysterious ore with dark, magical power, be careful with this one"}}},
            {"seedsCabbage",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "seedsCabbage" }, {"toolType", "seed"}, {"displayName", "cabbage seeds"}, {"shopCost", "2"},
                                                              {"exp", "0"}, {"itemDescription", "Seeds for growing bountiful cabbage"}}},
            {"seedsCarrot",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "seedsCarrot" }, {"toolType", "seed"}, {"displayName", "carrot seeds"}, {"shopCost", "5"},
                                                             {"exp", "0"}, {"itemDescription", "Seeds for growing vibrant carrots"}}},
            {"seedsTomato",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "seedsTomato" }, {"toolType", "seed"}, {"displayName", "tomato seeds"}, {"shopCost", "15"},
                                                              {"exp", "0"}, {"itemDescription", "Seeds for growing bursting tomatos"}}},
            {"seedsGreenBean",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "seedsGreenBean" }, {"toolType", "seed"}, {"displayName", "green bean seeds"}, {"shopCost", "20"},
                                                                {"exp", "0"}, {"itemDescription", "Seeds for growing long skinny green beans"}}},
            {"seedsCorn",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "seedsCorn" }, {"toolType", "seed"}, {"displayName", "corn seeds"}, {"shopCost", "10"},
                                                           {"exp", "0"}, {"itemDescription", "Seeds for growing some husky corn"}}},
            {"seedsSquash",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "seedsSquash" }, {"toolType", "seed"}, {"displayName", "squash seeds"}, {"shopCost", "30"},
                                                             {"exp", "0"}, {"itemDescription", "Seeds for growing a plump squash"}}},
            {"seedsWheat",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "seedsWheat" }, {"toolType", "seed"}, {"displayName", "wheat seeds"}, {"shopCost", "25"},
                                                            {"exp", "0"}, {"itemDescription", "Seeds for growing some whispy wheat"}}},
            {"cabbage",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "cabbage" }, {"toolType", "food"}, {"displayName", "cabbage"}, {"shopCost", "20"},
                                                         {"exp", "25"}, {"itemDescription", "Who eats this stuff anyways?"}}},
            {"carrot",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "carrot" }, {"toolType", "food"}, {"displayName", "carrot"}, {"shopCost", "30"},
                                                        {"exp", "50"}, {"itemDescription", "Don't eat too many or you'll turn...orange"}}},
            {"corn",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "corn" }, {"toolType", "food"}, {"displayName", "corn"}, {"shopCost", "40"},
                                                      {"exp", "100"}, {"itemDescription", "Should you eat corn that has fallen off the stalk?"}}},
            {"tomato",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "tomato" }, {"toolType", "food"}, {"displayName", "tomato"}, {"shopCost", "50"},
                                                        {"exp", "200"}, {"itemDescription", "Red and pasty, tomatos will make your dish tasty"}}},
            {"wheat",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "wheat" }, {"toolType", "food"}, {"displayName", "wheat"}, {"shopCost", "20"},
                                                       {"exp", "400"}, {"itemDescription", "Wheat, the staple of food"}}},
            {"greenBean",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "greenBean" }, {"toolType", "food"}, {"displayName", "green beans"}, {"shopCost", "60"},
                                                           {"exp", "800"}, {"itemDescription", "Lean and green, add some fiber to your dish"}}},
            {"squash",new Dictionary<string, string>() {{"prefabName", "axe"}, {"toolName", "squash" }, {"toolType", "food"}, {"displayName", "squash"}, {"shopCost", "80"},
                                                        {"exp", "1600"}, {"itemDescription", "They're mostly fun for throwing on cement"}}}
        };

        objectsDict = new Dictionary<string, Dictionary<string, string>>()
        {
            { "treeNormal", new Dictionary<string, string>() { { "prefabName", "treeNormal" }, { "objectName", "treeNormal" }, { "objectType", "tree" }, { "displayName", "normal tree" }, { "objectTag", "treeNormal" }}},
            { "treeBirch", new Dictionary<string, string>() { { "prefabName", "treeBirch" }, { "objectName", "treeBirch" }, { "objectType", "tree" }, { "displayName", "birch tree" }, { "objectTag", "treeBirch" }}},
            { "rockNormal", new Dictionary<string, string>() { { "prefabName", "rockNormal" }, { "objectName", "rockNormal" }, { "objectType", "ore" }, { "displayName", "rock" }, { "objectTag", "rockNormal" }}},
            { "oreCopper", new Dictionary<string, string>() { { "prefabName", "oreCopper" }, { "objectName", "oreCopper" }, { "objectType", "ore" }, { "displayName", "copper ore" }, { "objectTag", "oreCopper" }}},
            { "oreIron", new Dictionary<string, string>() { { "prefabName", "oreIron" }, { "objectName", "oreIron" }, { "objectType", "ore" }, { "displayName", "iron ore" }, { "objectTag", "oreIron" }}},
            { "oreGold", new Dictionary<string, string>() { { "prefabName", "oreGold" }, { "objectName", "oreGold" }, { "objectType", "ore" }, { "displayName", "gold ore" }, { "objectTag", "oreGold" }}},
            { "orePlatinum", new Dictionary<string, string>() { { "prefabName", "orePlatinum" }, { "objectName", "orePlatinum" }, { "objectType", "ore" }, { "displayName", "platinum ore" }, { "objectTag", "orePlatinum" }}},
            { "oreGemstone", new Dictionary<string, string>() { { "prefabName", "oreGemstone" }, { "objectName", "oreGemstone" }, { "objectType", "ore" }, { "displayName", "gemstone ore" }, { "objectTag", "oreGemstone" }}},
            { "oreDark", new Dictionary<string, string>() { { "prefabName", "oreDark" }, { "objectName", "oreDark" }, { "objectType", "ore" }, { "displayName", "dark ore" }, { "objectTag", "oreDark" }}}
        };
    }
    


    public struct SkillData
    {
        public string skillName;
        public int[] newTierLevels;
        public string[] newTierItems;
        public int[] newTierRewards; // Gold
        public string[] itemDisplayName;

        public SkillData(string skillName, int[] newTierLevels, string[] newTierItems, int[] newTierRewards, string[] itemDisplayName)
        {
            this.skillName = skillName;
            this.newTierLevels = newTierLevels;
            this.newTierItems = newTierItems;
            this.newTierRewards = newTierRewards;
            this.itemDisplayName = itemDisplayName;
        }
    }

    public static class AllSkillsData
    {
        public static SkillData farming = new SkillData("farming",
                                                        new int[] { 1, 5, 10, 20, 30, 40, 50 },
                                                        new string[] { "cabbage", "wheat", "carrot", "corn", "tomato", "greenBean", "squash" },
                                                        new int[] { 100, 200, 400, 1000, 2000, 5000, 20000 }, 
                                                        new string[] { "Cabbage", "Wheat", "Carrots", "Corn", "Tomato", "Green Beans", "Squash" });
        public static SkillData woodcutting = new SkillData("woodcutting",
                                                        new int[] { 1, 5, 10, 20, 30, 40, 50 },
                                                        new string[] { "treeNormal", "treeBirch", "treeBirch", "treeBirch", "treeBirch", "treeBirch", "treeBirch" },
                                                        new int[] { 100, 200, 400, 1000, 2000, 5000, 20000 }, 
                                                        new string[] { "Normal", "Birch", "Birch", "Birch", "Birch", "Birch", "Birch" });
        public static SkillData cooking = new SkillData("cooking",
                                                        new int[] { 1, 5, 10, 20, 30, 40, 50 },
                                                        new string[] { "cabbage", "cabbage", "cabbage", "cabbage", "cabbage", "cabbage", "cabbage" },
                                                        new int[] { 100, 200, 400, 1000, 2000, 5000, 20000 },
                                                        new string[] { "Cabbage", "Cabbage", "Cabbage", "Cabbage", "Cabbage", "Cabbage", "Cabbage" } );
        public static SkillData mining = new SkillData("mining",
                                                        new int[] { 1, 5, 10, 20, 30, 40, 50 },
                                                        new string[] { "rockNormal", "oreCopper", "oreIron", "oreGold", "orePlatinum", "oreGemstone", "oreDark" },
                                                        new int[] { 100, 200, 400, 1000, 2000, 5000, 20000 },
                                                        new string[] { "Rock", "Copper", "Iron", "Gold", "Platinum", "Gemstone", "Dark" });

        public static SkillData[] SkillsArray = new SkillData[] { farming, woodcutting, cooking, mining }; 
    }

}