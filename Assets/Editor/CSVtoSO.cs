using UnityEngine;
using System.IO;
using UnityEditor;
using System.Collections.Generic;

public class CSVtoSO
{
    private static string enemyCSVPath = "/Editor/CSVs/EnemyDataCSV.csv";

    [MenuItem("Utilities/Generate Enemies")]
    public static void GenerateEnemies()
    {
        string[] allLines = File.ReadAllLines(Application.dataPath + enemyCSVPath);

        foreach (string s in allLines)
        {
            string[] splitData = s.Split(new char[] { ',' }, System.StringSplitOptions.None);

            if (splitData.Length != 21)
            {
                Debug.LogWarning($"Dòng dữ liệu không hợp lệ: {s}. Có {splitData.Length} giá trị thay vì 21.");
                continue;
            }

            EnemyData enemy = ScriptableObject.CreateInstance<EnemyData>();
            enemy.enemyName = splitData[1];

            // Kiểm tra và chuyển đổi Tier
            if (int.TryParse(splitData[5], out int parsedTier))
            {
                enemy.tier = parsedTier;
            }
            else
            {
                Debug.LogWarning($"Không thể parse Tier: {splitData[5]} cho enemy {enemy.enemyName}, đặt mặc định là 0.");
                enemy.tier = 0; // Đặt mặc định nếu lỗi
            }

            // Kiểm tra và chuyển đổi maxHp
            if (float.TryParse(splitData[2], out float parsedHp))
            {
                enemy.maxHp = parsedHp;
            }
            else
            {
                Debug.LogWarning($"Không thể parse HP: {splitData[2]} cho enemy {enemy.enemyName}, đặt mặc định là 1.");
                enemy.maxHp = 1;
            }

            // Kiểm tra và chuyển đổi tốc độ di chuyển
            if (float.TryParse(splitData[3], out float parsedSpeed))
            {
                enemy.baseMoveSpeed = parsedSpeed;
            }
            else
            {
                Debug.LogWarning($"Không thể parse Speed: {splitData[3]} cho enemy {enemy.enemyName}, đặt mặc định là 1.");
                enemy.baseMoveSpeed = 1;
            }

            // Xử lý Element Enum
            if (System.Enum.TryParse(splitData[6], out Element parsedElement))
            {
                enemy.element = parsedElement;
            }
            else
            {
                Debug.LogWarning($"Không thể parse Element: {splitData[6]} cho enemy {enemy.enemyName}");
            }

            // Xử lý kỹ năng
            enemy.lvl1Abilities = new List<string>();
            enemy.lvl2Abilities = new List<string>();
            enemy.lvl3Abilities = new List<string>();

            if (!string.IsNullOrEmpty(splitData[7])) enemy.lvl1Abilities.Add(splitData[7]);
            if (!string.IsNullOrEmpty(splitData[9])) enemy.lvl1Abilities.Add(splitData[9]);
            if (!string.IsNullOrEmpty(splitData[11])) enemy.lvl1Abilities.Add(splitData[11]);

            if (!string.IsNullOrEmpty(splitData[13])) enemy.lvl2Abilities.Add(splitData[13]);
            if (!string.IsNullOrEmpty(splitData[15])) enemy.lvl2Abilities.Add(splitData[15]);
            if (!string.IsNullOrEmpty(splitData[17])) enemy.lvl2Abilities.Add(splitData[17]);

            if (!string.IsNullOrEmpty(splitData[19])) enemy.lvl3Abilities.Add(splitData[19]);

            AssetDatabase.CreateAsset(enemy, $"Assets/Resources/EnemyData/{splitData[0]}.asset");
        }

        AssetDatabase.SaveAssets();
    }
}
