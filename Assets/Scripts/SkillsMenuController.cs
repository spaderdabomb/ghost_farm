using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using System;
using UnityEngine.UI;

public class SkillsMenuController : MonoBehaviour
{
    [SerializeField] GameSceneController gameSceneController;

    [SerializeField] GameObject skillsMenu;
    [SerializeField] GameObject craftingScrollViewContent;
    [SerializeField] GameObject skillSelectedTitle;
    [SerializeField] GameObject farmingExpText;
    [SerializeField] GameObject woodcuttingExpText;
    [SerializeField] GameObject cookingExpText;
    [SerializeField] GameObject miningExpText;
    [SerializeField] GameObject farmingLevelText;
    [SerializeField] GameObject woodcuttingLevelText;
    [SerializeField] GameObject cookingLevelText;
    [SerializeField] GameObject miningLevelText;
    [SerializeField] GameObject expColorbarFarming;
    [SerializeField] GameObject expColorbarWoodcutting;
    [SerializeField] GameObject expColorbarCooking;
    [SerializeField] GameObject expColorbarMining;

    public int currentSkillIndex;

    private void Awake()
    {
        currentSkillIndex = 0;
    }

    void Start()
    {
        UpdateUI();
        AddExpToSkill("farming", 0);
        AddExpToSkill("woodcutting", 0);
        AddExpToSkill("cooking", 0);
        AddExpToSkill("mining", 0);
    }

    public void UpdateUI()
    {
        skillSelectedTitle.GetComponent<Text>().text = GlobalData.AllSkillsData.SkillsArray[currentSkillIndex].skillName.ToUpper();

        foreach (Transform child in craftingScrollViewContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        GameObject skillsMenuLevelLabel = Resources.Load("Prefabs/skillsMenuLevelLabel") as GameObject;
        GameObject skillsMenuItemLabel = Resources.Load("Prefabs/skillsMenuItemLabel") as GameObject;
        GameObject skillsMenuRewardLabel = Resources.Load("Prefabs/skillsMenuRewardLabel") as GameObject;

        int[] gameDataExpArray = new int[] { GameData.instance.farmingExp, GameData.instance.woodcuttingExp, GameData.instance.cookingExp, GameData.instance.miningExp };
        int[] gameDataLevelArray = new int[] { GameData.instance.farmingLevel, GameData.instance.woodcuttingLevel, GameData.instance.cookingLevel, GameData.instance.miningLevel };
        bool[][] gameDataRewardBoolArray = new bool[][] { GameData.instance.farmingRewardClaimed, GameData.instance.woodcuttingRewardClaimed, GameData.instance.cookingRewardClaimed, GameData.instance.miningRewardClaimed };

        for (int i = 0; i < GlobalData.AllSkillsData.SkillsArray[currentSkillIndex].newTierLevels.Length; i++)
        {
            // Instantiate game objects
            GameObject instantiatedSkillsMenuLevelLabel = Instantiate(skillsMenuLevelLabel, Vector3.zero, Quaternion.identity);
            GameObject instantiatedSkillsMenuItemLabel = Instantiate(skillsMenuItemLabel, Vector3.zero, Quaternion.identity);
            GameObject instantiatedSkillsMenuRewardLabel = Instantiate(skillsMenuRewardLabel, Vector3.zero, Quaternion.identity);

            // Adjust labels and images
            GameObject levelNumberLabel = instantiatedSkillsMenuLevelLabel.transform.GetChild(0).gameObject;
            levelNumberLabel.GetComponent<Text>().text = GlobalData.AllSkillsData.SkillsArray[currentSkillIndex].newTierLevels[i].ToString();
            instantiatedSkillsMenuItemLabel.GetComponent<Text>().text = GlobalData.AllSkillsData.SkillsArray[currentSkillIndex].itemDisplayName[i];
            GameObject itemImage = instantiatedSkillsMenuItemLabel.transform.GetChild(0).gameObject;
            itemImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/" + GlobalData.AllSkillsData.SkillsArray[currentSkillIndex].newTierItems[i]);
            GameObject rewardImage = instantiatedSkillsMenuRewardLabel.transform.GetChild(0).gameObject;

            // Change reward based on level progression
            if (gameDataLevelArray[currentSkillIndex] >= GlobalData.AllSkillsData.SkillsArray[currentSkillIndex].newTierLevels[i] && !gameDataRewardBoolArray[currentSkillIndex][i])
            {
                rewardImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ChestReward");
            }
            else if (gameDataLevelArray[currentSkillIndex] <= GlobalData.AllSkillsData.SkillsArray[currentSkillIndex].newTierLevels[i] && gameDataRewardBoolArray[currentSkillIndex][i])
            {
                rewardImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/ChestReward");
                rewardImage.GetComponent<Image>().color = new Color32(255, 255, 255, 102);
            }
            else
            {
                rewardImage.GetComponent<Image>().sprite = Resources.Load<Sprite>("Sprites/Lock");
            }

            // Set parent of instantiated prefabs
            instantiatedSkillsMenuLevelLabel.transform.SetParent(craftingScrollViewContent.transform, false);
            instantiatedSkillsMenuItemLabel.transform.SetParent(craftingScrollViewContent.transform, false);
            instantiatedSkillsMenuRewardLabel.transform.SetParent(craftingScrollViewContent.transform, false);

            GameObject buttonGameObj = instantiatedSkillsMenuRewardLabel.transform.GetChild(0).gameObject;
            int tempIndex = i;
            buttonGameObj.GetComponent<Button>().onClick.AddListener(delegate { RewardClaimedClicked(instantiatedSkillsMenuRewardLabel.transform.GetChild(0).gameObject, tempIndex); });
        }
    }

    public void UpdateSkillSelectedIndex(int index)
    {
        currentSkillIndex = index;
        UpdateUI();
    } 

    public void AddExpToSkill(string skillName, int exp)
    {
        if (skillName == "farming")
        {
            GameData.instance.farmingExp += exp;
            int totalLevelExp = 0;
            for (int i = 0; i + 1 < GameData.instance.farmingLevel; i++) { totalLevelExp += GlobalData.levelUpExp[i]; }
            int currentExp = GameData.instance.farmingExp - totalLevelExp;
            if (currentExp > GlobalData.levelUpExp[GameData.instance.farmingLevel - 1])
            {
                currentExp -= GlobalData.levelUpExp[GameData.instance.farmingLevel - 1];
                GameData.instance.farmingLevel += 1;
            }
            GameData.instance.Save();
            farmingExpText.GetComponent<Text>().text = currentExp.ToString() + "/" + GlobalData.levelUpExp[GameData.instance.farmingLevel - 1];
            farmingLevelText.GetComponent<Text>().text = "Lv " + GameData.instance.farmingLevel.ToString();
            print(252f * (currentExp / GlobalData.levelUpExp[GameData.instance.farmingLevel - 1]));
            expColorbarFarming.GetComponent<RectTransform>().sizeDelta = new Vector2(252f * ((float)currentExp / (float)GlobalData.levelUpExp[GameData.instance.farmingLevel - 1]), 24f);
        }
        else if (skillName == "woodcutting")
        {
            GameData.instance.woodcuttingExp += exp;
            int totalLevelExp = 0;
            for (int i = 0; i + 1 < GameData.instance.woodcuttingLevel; i++) { totalLevelExp += GlobalData.levelUpExp[i]; }
            int currentExp = GameData.instance.woodcuttingExp - totalLevelExp;
            if (currentExp >= GlobalData.levelUpExp[GameData.instance.woodcuttingLevel - 1])
            {
                currentExp -= GlobalData.levelUpExp[GameData.instance.woodcuttingLevel - 1];
                GameData.instance.woodcuttingLevel += 1;
            }
            GameData.instance.Save();
            woodcuttingExpText.GetComponent<Text>().text = currentExp.ToString() + "/" + GlobalData.levelUpExp[GameData.instance.woodcuttingLevel - 1];
            woodcuttingLevelText.GetComponent<Text>().text = "Lv " + GameData.instance.woodcuttingLevel.ToString();
            expColorbarWoodcutting.GetComponent<RectTransform>().sizeDelta = new Vector2(252f * ((float)currentExp / (float)GlobalData.levelUpExp[GameData.instance.woodcuttingLevel - 1]), 24f);
        }
        else if (skillName == "cooking")
        {
            GameData.instance.cookingExp += exp;
            int totalLevelExp = 0;
            for (int i = 0; i + 1 < GameData.instance.cookingLevel; i++) { totalLevelExp += GlobalData.levelUpExp[i]; }
            int currentExp = GameData.instance.cookingExp - totalLevelExp;
            if (currentExp >= GlobalData.levelUpExp[GameData.instance.cookingLevel - 1])
            {
                currentExp -= GlobalData.levelUpExp[GameData.instance.cookingLevel - 1];
                GameData.instance.cookingLevel += 1;
            }
            GameData.instance.Save();
            cookingExpText.GetComponent<Text>().text = currentExp.ToString() + "/" + GlobalData.levelUpExp[GameData.instance.cookingLevel - 1];
            cookingLevelText.GetComponent<Text>().text = "Lv " + GameData.instance.cookingLevel.ToString();
            expColorbarCooking.GetComponent<RectTransform>().sizeDelta = new Vector2(252f * ((float)currentExp / (float)GlobalData.levelUpExp[GameData.instance.cookingLevel - 1]), 24f);
        }
        else if (skillName == "mining")
        {
            GameData.instance.miningExp += exp;
            GameData.instance.Save();
            int totalLevelExp = 0;
            for (int i = 0; i + 1 < GameData.instance.miningLevel; i++) { totalLevelExp += GlobalData.levelUpExp[i]; }
            int currentExp = GameData.instance.miningExp - totalLevelExp;
            if (currentExp >= GlobalData.levelUpExp[GameData.instance.miningLevel - 1])
            {
                currentExp -= GlobalData.levelUpExp[GameData.instance.miningLevel - 1];
                GameData.instance.miningLevel += 1;
            }
            GameData.instance.Save();
            miningExpText.GetComponent<Text>().text = currentExp.ToString() + "/" + GlobalData.levelUpExp[GameData.instance.miningLevel - 1];
            miningLevelText.GetComponent<Text>().text = "Lv " + GameData.instance.miningLevel.ToString();
            expColorbarMining.GetComponent<RectTransform>().sizeDelta = new Vector2(252f * ((float)currentExp / (float)GlobalData.levelUpExp[GameData.instance.miningLevel - 1]), 24f);
        }
    }

    public void RewardClaimedClicked(GameObject button, int rewardIndex)
    {
        print(rewardIndex);
    }

}
