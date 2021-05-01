export class Language {
 public nom: string;
 public niveauParle:string;
 public niveauEcrit:string;
 public niveauCompris:string;
 public isAppCurrentLanguage:boolean=false;
 public isCompulsoryLanguage:boolean=false;
 constructor(
   nom:string,
   niveauParle:string,
   niveauEcrit:string,
   niveauCompris:string,
   isappCurrentLang:boolean=false,
   isCompulsory:boolean=false
  ){
    this.nom = nom;
    this.niveauEcrit = niveauEcrit;
    this.niveauParle = niveauParle;
    this.niveauCompris = niveauCompris;
    this.isAppCurrentLanguage = isappCurrentLang;
    this.isCompulsoryLanguage = isCompulsory;
  }
}

export class NewLanguageVM {
  public selected_name_id: string;
  public selected_talk_id:string;
  public selected_write_id:string;
  public selected_understand_id:string;
  constructor(
    name_id:string,
    talk_id:string,
    write_id:string,
    understand_id:string,
   ){
     this.selected_name_id = name_id;
     this.selected_talk_id = talk_id;
     this.selected_write_id = write_id;
     this.selected_understand_id = understand_id;
   }
 }
