# Sonidos En Unity

## 1. Añadir un audio que se debe reproducir en cuanto se carga la escena y en bucle.

Creamos un objeto y le añadimos el componente **AudioSource** a la escena y añadimos en el apartado *AudioClip* el audio que queremos reproducir. Para que se reproduzca el audio nada mas comenzar la escena y en bucle debemos seleccionar las opciones de *Play On Awake* y *Loop*

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/1.%20Fuente%20de%20sonido.png)

https://github.com/user-attachments/assets/0b4ce47a-765e-4359-9205-18865e9ad781

## 2. Crea un objeto con una fuente de audio a la que le configures el efecto Doppler elevado y que se mueva a al pulsar la tecla m a una velocidad alta.

Añadimos una esfera en la escena y le añadimos un **AudioSource** que tendrá la propiedad Spatial Blend a 1 para que Unity lo trate como un audio 3D. Luego, realizamos los siguientes cambios:

- Incrementamos el valor de Doppler Level para alternar el pitch en función de la velocidad del GameObject.
- Incrementamos el valor de Spread para regular el audio estéreo.
- Incrementamos los valores de Min Distance y Max Distance.
- Cambiamos la propiedad Volume Rolloff a Linear Rolloff. Al cambiarlo a este modo, entre más lejos estemos de la fuente que reproduce el sonido, menos escucharemos.

Resultado con **Logarithmic Rolloff**

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/3.%20Logarithmic%20Rolloff.png)

https://github.com/user-attachments/assets/0c8d6bde-1a25-4e41-889f-f7dc33346e9b

Resultado con **Linear Rolloff**

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/5.%20Linear%20Rolloff.png)

https://github.com/user-attachments/assets/74346ad3-52b6-4ea9-8c0b-04acc6b28609

## 3.Configurar un mezclador de sonidos, aplica a uno de los grupo un filtro de eco y el resto de filtros libre. Configura cada grupo y masteriza el efecto final de los sonidos que estás mezclando. Explica los cambios que has logrado con tu mezclador.

Creamos un AudioMixer dentro de donde tenemos la carpeta assets, y seleccionando la opción **Create -> Audio Mixer**

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/6.Creacion%20Audio%20Mixer.png)

Una vez hecho, creamos los grupos con los que trabajaremos

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/7.%20%20AudioMixer%20Grupos.png)

En el inspector del grupo Echo, añadimos el efecto para que el sonido del eco decaiga a lo largo del tiempo, para ello modificamos los valores de *Decay* y *Drymix* al 50%.

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/8.%20Echo%20inspector.png)

Por último, añadimos los grupos a los *AudioSources* respectivos

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/9.%20A%C3%B1adiendo%20grupos.png)

Resultado con **Mixer** Activado

https://github.com/user-attachments/assets/bcba14bd-c1d2-40b0-a037-585aecaea849

Resultado con **Mixer** Desactivado

https://github.com/user-attachments/assets/1dd87ca6-8d00-46b7-9f6b-e8dcfe6a39e4

## 4. Implementar un script que al pulsar la tecla P accione el movimiento de una esfera en la escena y reproduzca un sonido en bucle hasta que se pulse la tecla S.

Debido a un problema de Unity de no reconocer la tecla S, se sustituyó con la tecla D

```c#
public class SceneAudio : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 10.0f;

    [SerializeField] private AudioSource _audioSource;

    private bool _isMovementEnabled;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            _isMovementEnabled = !_isMovementEnabled;

            if (!_audioSource.isPlaying)
            {
                Debug.Log($"Letra <P> presionada. Reproduciendo audio: {_audioSource.clip.name}");
                _audioSource.Play();
            }
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            if (_audioSource.isPlaying)
            {
                Debug.Log($"Letra <D> presionada. Parando audio: {_audioSource.clip.name}");
                _audioSource.Stop();
            }
        }

        if (_isMovementEnabled)
            transform.Translate(_moveSpeed * Time.deltaTime, 0, 0);
    }
}
```

https://github.com/user-attachments/assets/99cbf7aa-a3ec-4989-97dc-8f1f4c92e3f5

## 5. Implementar un script en el que el jugador active un sonido al colisionar con la esfera.

Se añaden los componentes de **SphereCollider** y **AudioSource** a la esfera y el siguiente script.

```c#
public class ObjectCollision : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_audioSource.isPlaying)
                _audioSource.Play();
        }
    }
}
```

https://github.com/user-attachments/assets/3074012c-14dc-4bb0-b5b2-d501cb97d11b

## 6. Modificar el script anterior para que según la velocidad a la que se impacte, el cubo lance un sonido más fuerte o más débil.

```c#
public class ObjectCollision2 : MonoBehaviour
{
    private AudioSource _audioSource;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.volume = other.gameObject.GetComponent<PlayerMovement>().MoveSpeed / 100;
                _audioSource.Play();
            }
        }
    }
}
```

https://github.com/user-attachments/assets/d3e73c23-9e45-4e6e-99a1-9dce53406ab8

## 7. Agregar un sonido de fondo a la escena que se esté reproduciendo continuamente desde que esta se carga. Usar un mezclador para los sonidos.

Este apartado es similar al apartado 3, en el cual se usa un mixer para reproducir una música de fondo y otro de sonidos.

## 8. Crear un script para simular el sonido que hace el jugador cuando está en movimiento (mecánica para reproducir sonidos de pasos).

Añadimos un **AudioSource** al personaje y desmarcamos las opciones *Play On Awake* y *Loop*. También es importante dejar la referencia de AudioClip sin asignar, ya que la asignaremos directamente desde el código.

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/15.%20AudioSource%20pasos%20player.png)

A continuación, creamos un array de AudioClip en el script que serán reproducidos mientras el personaje se mueve.

```c#
public class PlayerWalkAudio : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;

    [SerializeField] private AudioClip[] audioClips;
    
    private AudioSource _audioSource;
    private Random _random;
    
    public float MoveSpeed => moveSpeed;

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _random = new Random();
    }

    void Update()
    {
        var horizontalMovement = Input.GetAxis("Horizontal");
        var verticalMovement = Input.GetAxis("Vertical");

        if (horizontalMovement != 0 || verticalMovement != 0)
        {
            if (!_audioSource.isPlaying)
            {
                _audioSource.clip = audioClips[_random.Next(0, audioClips.Length - 1)];
                _audioSource.Play();
            }
            transform.Translate(Input.GetAxis("Horizontal") * moveSpeed * Time.deltaTime, 0, Input.GetAxis("Vertical") * moveSpeed * Time.deltaTime);
        }
        
}
```

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/16.%20Pasos%20a%C3%B1adidos.png)

https://github.com/user-attachments/assets/e8f158ab-2bbd-40c6-82f6-bef488c60bd7

## 9. En la escena de tus ejercicios 2D incorpora efectos de sonido ajustados a los siguientes requisitos:

**1. Crea un grupo SFX en el AudioMixer para eventos:**
 - Movimiento del personaje: Crea sonidos específicos para saltos y aterrizajes.
 - Interacción y recolección de objetos: Diseña sonido para la recolección de objetos.
 - Indicadores de salud/vida: Diseña un sonido breve y distintivo para cada cambio en el estado de salud (por ejemplo, ganar o perder vida).

**2. Crea un grupo Ambiente:**
 - Crea un sonido de fondo acorde con el ambiente
 - Agrega una zona específica del juego en que el ambiente cambie de sonido

**3. Crea un grupo para música:**
 - Crea un loop de música de fondo acorde al tono del juego

![](https://github.com/Alu0101030562/Screenshots/blob/main/Screenshots/Sonido/19.%20AudioMixer2D.png)
 
https://github.com/user-attachments/assets/ef5b7c96-450d-4ed3-bf15-6c047b635d2e




