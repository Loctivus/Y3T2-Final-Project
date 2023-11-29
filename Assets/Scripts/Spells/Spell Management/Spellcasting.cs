using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Spellcasting : MonoBehaviour
{
    #region Variables
    public BoolRef gamePaused;
    [Tooltip("Current list of spells able to be cast.")]
    public List<GameObject> currentList = new List<GameObject>();

    //Dictionary for storing keybinds for number keys along top of alphanumerical keyboard and int values for changing spells
    Dictionary<KeyCode, int> spellKeybinds = new Dictionary<KeyCode, int>()
    {
        { KeyCode.Alpha1, 0 },
        { KeyCode.Alpha2, 1 },
        { KeyCode.Alpha3, 2 },
        { KeyCode.Alpha4, 3 },
        { KeyCode.Alpha5, 4 },
    };

    [Tooltip("List of TMP_Text components that are above spell icons on bottom bar of canvas")]
    public List<TMP_Text> spellKeyTexts = new List<TMP_Text>();

    public PlayerStatsVar playerStatChecker;
    public BoolVariable casting;
    [Tooltip("Transform scriptable object to track position and pass that info to other objects via TransformRef references in respective scripts")]
    public TransformVariable spellHandTFSO;
    public Transform spellHand;
    public TMP_Text spellText;
    public Image spellImage; 
    //public Vector3Variable castDirectionPoint;
    GameObject heldSpell;
    Spell spellRef;

    Animator anim;
    AudioSource audioSource;
    [SerializeField] int spellNum = 0;

    #endregion

    private void Awake()
    {
        casting.value = false;
        anim = GetComponentInChildren<Animator>();
        audioSource = GetComponent<AudioSource>();
        ChangeSpell(0);
    }

    /// <summary>
    /// On left click, if not casting, reduce mana by amount specified by respective spell stat
    /// Set casting value to true to prevent animator trigger spam
    /// Set animator trigger to play cast animation
    /// Cast animation specified by 'Spell Anim Int' in spell stat scriptable object
    /// 
    /// On mouse scroll, increase or decrease spellNum and change spell based on new spellNum value
    /// Set text colour of spellKey texts to white to show they are not the current spell selected
    /// When pressing any keys set in keybind dictionary, get the value assigned to that key and set spellNum as that value then change spell in accordance with that value
    /// </summary>
    private void Update()
    {
        if (!gamePaused.value)
        {
            if (Input.GetMouseButtonDown(0) && !casting.value)
            {
                spellRef = currentList[spellNum].GetComponent<Spell>();

                if ((playerStatChecker.playerMP - spellRef.spellStats.manaCost) >= 0)
                {
                    playerStatChecker.playerMP -= spellRef.spellStats.manaCost;
                    casting.value = true;
                    anim.SetTrigger("SpellCast_trig");
                    
                }
            }

            if (Input.mouseScrollDelta.y > 0)
            {
                spellKeyTexts[spellNum].color = Color.white;
                spellNum++;
                ChangeSpell(spellNum);
            }
            else if (Input.mouseScrollDelta.y < 0)
            {
                spellKeyTexts[spellNum].color = Color.white;
                spellNum--;
                ChangeSpell(spellNum);
            }

            foreach(KeyValuePair<KeyCode, int> spellKeyPair in spellKeybinds)
            {
                if (Input.GetKeyDown(spellKeyPair.Key))
                {
                    spellKeyTexts[spellNum].color = Color.white;
                    spellNum = spellKeyPair.Value;
                    ChangeSpell(spellNum);
                }
            }
        }

        spellHandTFSO.value = spellHand;
    }

    public void ChangeSpell(int i)
    {
        if (i < 0)
        {
            spellNum = currentList.Count - 1;
        }
        else if (i >= currentList.Count)
        {
            spellNum = 0;
        }


        spellRef = currentList[spellNum].GetComponent<Spell>();
        anim.SetInteger("SpellNum_int", spellRef.spellStats.spellAnimInt);
        spellImage.sprite = spellRef.spellStats.spellImage;
        spellText.text = currentList[spellNum].name;
        spellKeyTexts[spellNum].color = Color.green;
        audioSource.clip = spellRef.spellStats.castSFX;
    }


    /// <summary>
    /// Set cast sound effect based on spell stat value
    /// Instantiate current selected spell and assign its parent to spellHand
    /// </summary>
    public void CastSpell()
    {
        audioSource.clip = spellRef.spellStats.castSFX;
        audioSource.Play();
        heldSpell = Instantiate(currentList[spellNum], spellHand.position, spellHand.rotation);
        heldSpell.transform.SetParent(spellHand);
        
    }



    /// <summary>
    /// Set casting value to false, allowing another spell to be cast
    /// set held spell parent to null
    /// Get spell component and call SpellReleased method to allow spell to carry out its specified functionality when released from the spell hand
    /// </summary>
    public void ReleaseSpell()
    {
        casting.value = false;
        anim.SetFloat("AnimSpeedMult_f", 1f);
        heldSpell.transform.SetParent(null);
        Spell releasedSpell = heldSpell.GetComponent<Spell>();
        releasedSpell.SpellReleased();
    }
}
