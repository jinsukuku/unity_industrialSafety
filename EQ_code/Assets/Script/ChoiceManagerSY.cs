using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using static UnityEngine.EventSystems.EventTrigger;
using UnityEngine.Video;

public class ChoiceManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;          // VideoPlayer ����
    public VideoClip nextVideoClip;          // �ٲ��� 'å�� �Ʒ�' ����

    // ������ ��ư
    public Button optionA;
    public Button optionB;
    public Button optionC;

    // "�ٽ� �����ϱ�" ��ư
    public Button retryButton;

    // "NEXT" ��ư
    public Button nextButton;
    private bool isNextStage = false; // NEXT�� �� �� �������� ����

    // ��ȭ �ؽ�Ʈ 
    public Text dialogueText;

    // UI �׷� ������Ʈ��
    public GameObject dialoguePanel;   // ��ȭâ �г� ��ü
    public GameObject optionPanel;     // ������ ��ư �׷� ��ü

    // �̹� ���õ� ��ư ������
    private bool aSelected = false;
    private bool cSelected = false;

    // ���� �ٽ� ���� ��� ������ üũ
    private bool isWaitForRetry = false;

    void Start()
    {
        // ó���� UI ���̵��� + Retry ��ư �����
        dialoguePanel.SetActive(true);
        optionPanel.SetActive(true);
        retryButton.gameObject.SetActive(false);

        // ���� �� ���� ù ��� ���
        dialogueText.text = "�Ӹ��� ��ȣ�� ������ Ȯ���߽��ϴ�.���� ���� ���� �� �ִ� ������ \n��Ҹ� �����ؾ� �մϴ�.���� ��Ҹ� ������ �ּ���.";

        // �� ��ư Ŭ�� �� SelectOption ���� (Coroutine ���/ ���ٽ�)
        optionA.onClick.AddListener(() => StartCoroutine(SelectOption("A", optionA)));
        optionB.onClick.AddListener(() => StartCoroutine(SelectOption("B", optionB)));
        optionC.onClick.AddListener(() => StartCoroutine(SelectOption("C", optionC)));


    }

    // ������ ��ư Ŭ�� �� 0.5�ʰ� ��� UI ��� ����� > ���� '�ν���...' ���� ǥ�� > �ʿ� ������ ���� ���� 
    IEnumerator SelectOption(string option, Button selectedButton)
    {
        // ���� ���� '�ٽ� �����ϱ�' ��� ���¸�, ���� ����
        if (isWaitForRetry) yield break;

        // ���õ� ��ư�� ��� ��Ȱ��ȭ�ϰ� ���� ���߱�
        selectedButton.interactable = false;
        Color faded = selectedButton.image.color;
        faded.a = 0.3f;
        selectedButton.image.color = faded;

        // UI �Ͻ������� �����
        dialoguePanel.SetActive(false);
        optionPanel.SetActive(false);
        retryButton.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        if (option == "B")
        {
            // SceneManager.LoadScene("SceneCh01"); // ���̵� > ���� �� ������ �����ϱ�

            dialoguePanel.SetActive(true);
            optionPanel.SetActive(false);
            retryButton.gameObject.SetActive(false); // �ٽü��� �����
            nextButton.gameObject.SetActive(true);   // NEXT ��ư ǥ��
            dialogueText.text = "�����̾�! ���� ������ ưư�� å�� ������ ����.";

            // NEXT ��ư Ŭ�� �� Ź�ھƷ� ����
            nextButton.onClick.RemoveAllListeners(); // ���� ���� �̺�Ʈ ������ ����
            nextButton.onClick.AddListener(OnNextClicked); // ��ư Ŭ�� �� ������ ���� ���� ����
        }
        else if ((option == "A" && !aSelected) || (option == "C" && !cSelected))
        {
            // ���� ����: �ٸ� ������ ����
            isWaitForRetry = true;

            // ��ȭâ�� Retry ��ư �ٽ� ǥ��
            dialoguePanel.SetActive(true);
            optionPanel.SetActive(true);
            retryButton.gameObject.SetActive(true);
            dialogueText.text = "���� ����⿡ �ʹ� �ν��� �� ����. �ٸ� ��Ҹ� ã�ƺ���.";

            if (option == "A") aSelected = true;
            else if (option == "C") cSelected = true;

            retryButton.onClick.RemoveAllListeners();
            retryButton.onClick.AddListener(() => Retry());
        }
    }

    // "�ٽ� �����ϱ�" ��ư Ŭ�� �� ó��
    void Retry()
    {
        // �ɼ� ��ư �ٽ� ���� ���� ���·� ��ȯ
        isWaitForRetry = false;

        retryButton.gameObject.SetActive(false);
        optionPanel.SetActive(true);
        dialogueText.text = "�ٸ� �������� ����.";
    }

    void OnNextClicked()
    {

        if (!isNextStage)
        {
            // �ؽ�Ʈ�� ����
            dialogueText.text = "������ ����� ������ ��ٸ��ϴ�.\n������ ��Ƶ�� Ź�ڿ��� ���� ������ Ȯ���մϴ�.";
            isNextStage = true; // ���� Ŭ������ �� ��ȯ�ϵ��� ǥ��

            // ���� ��ȯ > å�� �Ʒ�
            if (videoPlayer != null && nextVideoClip != null)
            {
                videoPlayer.Stop();
                videoPlayer.clip = nextVideoClip;
                videoPlayer.Play();
            }

        }
        else
        {
            // �� ��° NEXT Ŭ����, �� �̵�
            SceneManager.LoadScene("SceneIntro");
        }

    }


}
