export class ConfiguredLanguage {
 guid: string;
 language_name:string;
 level_speak_name:string;
 level_write_name:string;
 level_understand_name:string;
 isAppLanguage:boolean=false;
 isCompulsoryLanguage:boolean=false;
 constructor(
   guid:string,
   languageName:string,
   niveauParle:string,
   niveauEcrit:string,
   niveauCompris:string,
   isappCurrentLang:boolean=false,
   isCompulsory:boolean=false
  ){
    this.guid = guid;
    this.language_name = languageName;
    this.level_speak_name = niveauParle;
    this.level_write_name = niveauEcrit;
    this.level_understand_name = niveauCompris;
    this.isAppLanguage = isappCurrentLang;
    this.isCompulsoryLanguage = isCompulsory;
  }
}

export class AppLanguageGet{
  code:string
  constructor(code:string){
    this.code = code;
  }
}

export class SetAppLanguageResult{
  status:string
  constructor(status:string){
    this.status = status;
  }
}

export class NewLanguageVM {
  language_guid: string;
  speak_level_guid:string;
  write_level_guid:string;
  understand_level_guid:string;
  constructor(
    name_id:string,
    talk_id:string,
    write_id:string,
    understand_id:string,
   ){
     this.language_guid = name_id;
     this.speak_level_guid = talk_id;
     this.write_level_guid = write_id;
     this.understand_level_guid = understand_id;
   }
 }

 export class UserLanguage_Get{
  total_count:number;
  data:ConfiguredLanguage[];
  constructor(
     total: number,
      data:ConfiguredLanguage[]
   ){
     this.total_count=total;
     this.data=data;
   }
 }
export class Language{
  guid:string;
  name:string;
  code:string;
  description:string;
  constructor(
    guid:string,
    name:string,
    code:string,
    description:string
  ){
    this.guid=guid;
    this.name=name;
    this.code=code;
    this.description=description;
  }
}

export class Level{
  guid:string;
  name:string;
  constructor(
    guid:string,
    name:string
  ){
    this.guid=guid;
    this.name=name;
  }

}
