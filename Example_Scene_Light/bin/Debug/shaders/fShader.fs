//FRAGMENT SHADER
#version 330 core

//MATERIAL STRUCT

//The amount of color this object reflects under each type of light
struct Material {
    vec3 ambientStrength;
    vec3 diffuseStrength;
    vec3 specularStrength;
    float shine;
};

uniform Material material;
////////////////////////////////////////////////////////////////////////////////

//LIGHT STRUCT

struct Light {
    vec3 position;

    vec3 ambient;
    vec3 diffuse;
    vec3 specular;
};

uniform Light light;

////////////////////////////////////////////////////////////////////////////////


out vec4 fragColor;

uniform vec3 lightPos;

uniform vec3 camPos;

in vec3 fragNormal;
in vec3 fragPos;

void main()
{
    //DIFUSE//

    //Calculate Diffuse Light
    vec3 lightDir = normalize(lightPos - fragPos);
    vec3 normalizedFragNormal = normalize(fragNormal);

    //Value varies form 0 to 1
    float diffuseOffset = max(dot(normalizedFragNormal,lightDir),0.0f);

    vec3 diffuseLight = diffuseOffset * light.diffuse * material.diffuseStrength;

    ////////////////////////////////////////////////////////////////////////////

    //AMBIENT//

    //Calculate Ambient Light
    vec3 ambientLight = light.ambient * material.ambientStrength;

    ////////////////////////////////////////////////////////////////////////////

    //SPECULAR//

    //Calculate a vector that extends from the fragment towards the viewer
    vec3 viewDir = normalize(camPos - fragPos);

    //Calculate the vector of reflected light, from the fragment to the
    //viewer
    vec3 reflectLightDirection = reflect(-lightDir,normalizedFragNormal);

    //Calculate the impact of reflected light
    float specularOffset = pow(max(dot(reflectLightDirection,viewDir),0.0f), material.shine);

    //Calculate specularLight
    vec3 specularLight = specularOffset * light.specular * material.specularStrength;

    ////////////////////////////////////////////////////////////////////////////

    //Compute final color
    vec3 resultColor = diffuseLight + ambientLight + specularLight;

    //Apply color to each fragment
    fragColor = vec4(resultColor , 1.0);
}
