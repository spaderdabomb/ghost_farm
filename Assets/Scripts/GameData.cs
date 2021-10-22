using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

[Serializable]
class PlayerData
{
	public int numGold;
	public int farmingLevel;
	public int woodcuttingLevel;
	public int cookingLevel;
	public int miningLevel;
	public int farmingExp;
	public int woodcuttingExp;
	public int cookingExp;
	public int miningExp;

	public bool[] farmingRewardClaimed;
	public bool[] woodcuttingRewardClaimed;
	public bool[] cookingRewardClaimed;
	public bool[] miningRewardClaimed;
}
public class GameData : MonoBehaviour
{

	public static GameData instance = null;

	public int numGold;
	public int farmingLevel;
	public int woodcuttingLevel;
	public int cookingLevel;
	public int miningLevel;
	public int farmingExp;
	public int woodcuttingExp;
	public int cookingExp;
	public int miningExp;

	public bool[] farmingRewardClaimed;
	public bool[] woodcuttingRewardClaimed;
	public bool[] cookingRewardClaimed;
	public bool[] miningRewardClaimed;


	void Awake()
	{
		if (instance == null)
			instance = this;
		else if (instance != this)
			Destroy(gameObject);

		DontDestroyOnLoad(gameObject);

		InitGame();
	}

	public void Save()
	{
		BinaryFormatter bf = new BinaryFormatter();
		FileStream file = File.Create(Application.persistentDataPath + "/playerInfo.dat");
		PlayerData data = new PlayerData();

		// Data
		data.numGold = numGold;
		data.farmingLevel = farmingLevel;
		data.woodcuttingLevel = woodcuttingLevel;
		data.cookingLevel = cookingLevel;
		data.miningLevel = miningLevel;
		data.farmingExp = farmingExp;
		data.woodcuttingExp = woodcuttingExp;
		data.cookingExp = cookingExp;
		data.miningExp = miningExp;
		data.farmingRewardClaimed = farmingRewardClaimed;
		data.woodcuttingRewardClaimed = woodcuttingRewardClaimed;
		data.cookingRewardClaimed = cookingRewardClaimed;
		data.miningRewardClaimed = miningRewardClaimed;

		bf.Serialize(file, data);
		file.Close();
	}

	public void Load()
	{
		if (File.Exists(Application.persistentDataPath + "/playerInfo.dat"))
		{
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file = File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();

			// Data
			numGold = data.numGold;
            farmingLevel = data.farmingLevel;
			woodcuttingLevel = data.woodcuttingLevel;
			cookingLevel = data.woodcuttingLevel;
			miningLevel = data.farmingLevel;
			farmingExp = data.farmingExp;
			woodcuttingExp = data.woodcuttingExp;
			cookingExp = data.cookingExp;
			miningExp = data.miningExp;
			farmingRewardClaimed = data.farmingRewardClaimed;
			woodcuttingRewardClaimed = data.woodcuttingRewardClaimed;
			cookingRewardClaimed = data.cookingRewardClaimed;
			miningRewardClaimed = data.miningRewardClaimed;
		}

    }

	public void ResetData()
	{
		numGold = 80;
        farmingLevel = 1;
		woodcuttingLevel = 1;
		cookingLevel = 1;
		miningLevel = 1;
		farmingExp = 0;
		woodcuttingExp = 0;
		cookingExp = 0;
		miningExp = 0;
		farmingRewardClaimed = new bool[] { false, false, false, false, false, false, false };
		woodcuttingRewardClaimed = new bool[] { false, false, false, false, false, false, false };
		cookingRewardClaimed = new bool[] { false, false, false, false, false, false, false };
		miningRewardClaimed = new bool[] { false, false, false, false, false, false, false };

		GameData.instance.Save();
	}

	void InitGame()
	{
		GameData.instance.Load();
		GameData.instance.Save();
	}


	void Update()
	{

	}
}