using UnityEngine;

public class PuzzleBrankas : MonoBehaviour
{
    [Header("Dial Settings")]
    public Transform dial;
    public float rotationSpeed = 10f;
    public int totalNumbers = 60;
    public float snapAngle = 6f;
    public AudioSource clickSound;
    public AudioSource trueClickSound;

    [Header("Combination (0–60)")]
    public int[] combination = { 28, 11, 25 };
    public float tolerance = 0f;
    public AudioSource unlockSound;

    private int currentIndex = 0;
    private bool isUnlocked = false;
    private int targetNumber = 0;
    private int lastNumber = -1;
    private float nextInputTime = 0f;

    private float currentAngle = 0f;

    public float correctHoldTime = 1.5f; // berapa detik harus berhenti di angka benar
    private float holdTimer = 0f;
    public bool focused;

    void Update()
    {
        if (!focused) return;
        if (isUnlocked) return;

        float input = Input.GetAxis("Mouse X");

        // cuma boleh ganti angka tiap 0.1 detik biar kerasa mekanik
        if (Time.time >= nextInputTime)
        {
            if (input > 0.2f)
            {
                targetNumber++;
                nextInputTime = Time.time + 0.1f;
            }
            else if (input < -0.2f)
            {
                targetNumber--;
                nextInputTime = Time.time + 0.1f;
            }

            // wrap angka biar muter 0–59
            targetNumber = (targetNumber + totalNumbers) % totalNumbers;

            // bunyi klik dan log angka baru
            if (targetNumber != lastNumber)
            {
                if(clickSound != null)
                {
                    clickSound?.Play();
                }
                lastNumber = targetNumber;
                Debug.Log("Angka sekarang: " + targetNumber);
                CheckCombination(targetNumber);
            }

            // langsung snap ke angle baru (tanpa lerp)
            currentAngle = -targetNumber * snapAngle;
            dial.localEulerAngles = new Vector3(currentAngle, 0f, 0f);
        }

        CheckCombination(targetNumber);
    }


    void CheckCombination(int currentNumber)
    {
        if (currentIndex >= combination.Length) return;

        // kalau lagi di angka yang bener
        if (currentNumber == combination[currentIndex])
        {
            // kasih feedback pas baru nyentuh angka bener
            if (holdTimer <= 0f)
                Debug.Log($" Di angka yang benar: {combination[currentIndex]}, tahan posisi...");

            holdTimer += Time.deltaTime; // nambah waktu tiap frame

            // udah nahan cukup lama di angka bener
            if (holdTimer >= correctHoldTime)
            {
                Debug.Log($" Angka {combination[currentIndex]} dikonfirmasi benar!");
                trueClickSound.Play();
                currentIndex++;
                holdTimer = 0f; // reset timer

                if (currentIndex >= combination.Length)
                {
                    UnlockSafe();
                }
            }
        }
        else
        {
            // pindah angka = batalin timer
            if (holdTimer > 0f)
                Debug.Log(" Angka berubah, reset timer.");
            holdTimer = 0f;
        }
    }




    void UnlockSafe()
    {
        if (isUnlocked) return;
        isUnlocked = true;

        Debug.Log("BRANKAS KEBUKA BRO!");
        if (unlockSound != null)
        {
            unlockSound?.Play();
        }

        // TODO: animasi pintu safe kebuka di sini
        // doorAnimator.SetTrigger("Open");
    }
}
