namespace DictusClaudi.Core.LatinEndingsUtil;

public static class LatinEndingsUtil
{
    private static string AllEndings = "erunt,arum,orum,ibus,imus,ubus,erum,ebus,mini,ntur,amus,emus,etis,atis,itis,iunt,unt,abo,ant,ere,ium,uum,mus,tis,tur,mur,are,ent,nt,us,at,ae,am,is,as,um,os,em,im,es,eo,et,ua,ei,ia,ui,a,i,o,e,s,u,t";
    private static string Tackons = "neque,que,ne";

    public static string  GetParsedStem(string fullWord)
    {
        foreach(string tackon in Tackons.Split(','))
        {
            if (fullWord.EndsWith(tackon))
            {
                fullWord = fullWord.Substring(0, fullWord.Length - tackon.Length);
            }
        }

        foreach(string ending in AllEndings.Split(','))
        {
            if (fullWord.EndsWith(ending))
            {
                return fullWord.Substring(0, fullWord.Length - ending.Length);
            }
        }
        
        return fullWord;
    }
}
