using UnityEngine;

public class CRTRenderer : MonoBehaviour
{
    public RenderTexture CRTTexture;
    public Shader blitShader;

    private int _texID;
    private int _outputDimsID;
    private int _inputDimsID;
    private Material _blitMaterial;

    public void OnEnable() 
    {
        _texID = Shader.PropertyToID("_CRTTex");
        _outputDimsID = Shader.PropertyToID("_OutputDims");
        _inputDimsID = Shader.PropertyToID("_InputDims");

        _blitMaterial = new Material(blitShader);
    }
    
    public void OnRenderImage(RenderTexture _, RenderTexture dest)
    {
        // Set the shader properties
        _blitMaterial.SetTexture(_texID, CRTTexture);
        
        if (!ReferenceEquals(dest, null))
        {
            _blitMaterial.SetVector(_outputDimsID, new Vector2(dest.width, dest.height));
        }
        else
        {
            _blitMaterial.SetVector(_outputDimsID, new Vector2(Screen.width, Screen.height));
        }

        _blitMaterial.SetVector(_inputDimsID, new Vector2(CRTTexture.width, CRTTexture.height));

        Graphics.Blit(CRTTexture, dest, _blitMaterial);
    }
}