using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    private const float CAMERA_TRANSITION_SPEED = 3.0f;

    public GameObject levelButtonPrefab;
    public GameObject levelButtonContainer;

    public GameObject shopButtonPrefab;
    public GameObject shopButtonContainer;

    public Text currencyText;

    public Material playerMaterial;

    private Transform cameraTransform;
    private Transform cameraDesiredLookAt;

    private void Start()
    {
        ChangePlayerSkin(GameManager.Instance.currentSkinIndex);
        currencyText.text = "Currency : " + GameManager.Instance.currency.ToString();
        cameraTransform = Camera.main.transform;

        Sprite[] thumbnails = Resources.LoadAll<Sprite>("Levels");
        foreach (Sprite thumbnail in thumbnails)
        {
            GameObject container = Instantiate(levelButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = thumbnail;
            container.transform.SetParent(levelButtonContainer.transform, false);

            string sceneName = thumbnail.name;
            container.GetComponent<Button>().onClick.AddListener(() => LoadLevel(sceneName));
        }

        int textureIndex = 0;
        Sprite[] textures = Resources.LoadAll<Sprite>("Player");
        foreach (Sprite texture in textures)
        {
            GameObject container = Instantiate(shopButtonPrefab) as GameObject;
            container.GetComponent<Image>().sprite = texture;
            container.transform.SetParent(shopButtonContainer.transform, false);

            int index = textureIndex;
            container.GetComponent<Button>().onClick.AddListener(() => ChangePlayerSkin(index));
            if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index) {
                container.transform.GetChild(0).gameObject.SetActive(false);
            }
            textureIndex++;
        }
    }

    private void Update()
    {
        if (cameraDesiredLookAt != null)
        {
            cameraTransform.rotation = Quaternion.Slerp(cameraTransform.rotation, cameraDesiredLookAt.rotation, CAMERA_TRANSITION_SPEED * Time.deltaTime);
        }
    }

    private void LoadLevel(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void LookAtMenu(Transform menuTransform)
    {
        cameraDesiredLookAt = menuTransform;
    }

    private void ChangePlayerSkin(int index)
    {
        if ((GameManager.Instance.skinAvailability & 1 << index) == 1 << index)
        {

            float x = (index % 4) * 0.25f;
            float y = ((int)index / 4) * 0.25f;

            if (y == 0.0f)
            {
                y = 0.75f;
            }
            else if (y == 0.25f)
            {
                y = 0.5f;
            }
            else if (y == 0.50f)
            {
                y = 0.25f;
            }
            else if (y == 0.75f)
            {
                y = 0f;
            }

            playerMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));
            GameManager.Instance.currentSkinIndex = index;
            GameManager.Instance.Save();
        }

        else
        {
            //You do not have this skin, do you want to buy it?
            int cost = 150;

            if(GameManager.Instance.currency >= cost)
            {
                GameManager.Instance.currency -= cost;
                GameManager.Instance.skinAvailability += 1 << index;
                GameManager.Instance.Save();
                currencyText.text = "Currency : " + GameManager.Instance.currency.ToString();
                shopButtonContainer.transform.GetChild(index).GetChild(0).gameObject.SetActive(false);
                ChangePlayerSkin(index);
            }
        }

    }

}