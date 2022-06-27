using System.Xml;
using System.IO;
using UnityEditor;

public class OnGenerateCSProjectProcessor : AssetPostprocessor
{
    private static string OnGeneratedCSProject(string path, string content)
    {
        if (path.EndsWith("meltyStars.Hotfix.csproj"))
        {
            content = content.Replace("<Compile Include=\"Assets\\meltyStarsHotfix\\PlaceHolder.cs\" />", string.Empty);
            content = content.Replace("<None Include=\"Assets\\meltyStarsHotfix\\meltyStars.Hotfix.asmdef\" />", string.Empty);
        }
        if (path.EndsWith("meltyStars.Hotfix.csproj"))
        {
            return GenerateCustomCSProject(path, content, @"meltyStarsHotfix\**\*.cs");
        }
        return content;
    }
    private static string GenerateCustomCSProject(string path, string content, string hotfixPath)
    {
        XmlDocument document = new XmlDocument();
        document.LoadXml(content);

        var newDoc = document.Clone() as XmlDocument;

        var rootNode = newDoc.GetElementsByTagName("Project")[0];

        var itemGroup = newDoc.CreateElement("ItemGroup", newDoc.DocumentElement.NamespaceURI);
        var compile = newDoc.CreateElement("Compile", newDoc.DocumentElement.NamespaceURI);

        compile.SetAttribute("Include", hotfixPath);
        itemGroup.AppendChild(compile);

        rootNode.AppendChild(itemGroup);

        using (StringWriter sw = new StringWriter())
        {
            using (XmlTextWriter tx = new XmlTextWriter(sw))
            {
                tx.Formatting = Formatting.Indented;
                newDoc.WriteTo(tx);
                tx.Flush();
                return sw.GetStringBuilder().ToString();
            }
        }
    }
}
