using UnityEngine;

public class LUI_PAK : MonoBehaviour {

	[Header("VARIABLES")]
	public GameObject mainCanvas;
	public GameObject scriptObject;
	public Animator animatorComponent;
	public string animName;

	[Space]
	[Header("Game specific animations")]
	public AnimationPlayer mainMenuAnim;
	public AnimationPlayer playerAnim;

	void Start ()
	{
		animatorComponent.GetComponent<Animator>();
	}

    void Update ()
	{
		if (Input.anyKeyDown || Input.touchCount > 0) 
		{
			animatorComponent.Play (animName);
			mainCanvas.SetActive(true);
			scriptObject.SetActive(false);

			mainMenuAnim.PlayFromBegining("Menu In");
			playerAnim.PlayFromBegining("Move Out");
		}
	}
}