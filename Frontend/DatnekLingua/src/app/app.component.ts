///AUTHOR
///TAMKO STEPHANE,
///@contact: https://twitter.com/FlywingsS
import { HttpClient } from '@angular/common/http';
import { Component, Injectable, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Observable } from 'rxjs';
import { ConfiguredLanguage, Language, Level, NewLanguageVM, UserLanguage_Get, AppLanguageGet, SetAppLanguageResult } from './models';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  title = 'DatnekLingua';
  public newLanguage:NewLanguageVM;
  levels_parle:Level[]=[];
  levels_ecrit:Level[]=[];
  levels_comprehension:Level[]=[];
  languagesNames:Language[]=[];
  isModalActive = false;
  scrollDistance=1;
  throttle=50;
  selector="search-results";
  languageToDetail:any;
  total_configured_languages:number=0;
  languages:ConfiguredLanguage[]=[];
  closeResult: string="";
  page_size:number=10;
  availableLanguageCodeTranslations=[
    {"code":"fr", "name":"French"},
    {"code":"en-US", "name":"English"},
    {"code":"nl", "name":"Dutch"}
  ];
languageChangeObservable: any;

  constructor(private modalService: NgbModal, private http:HttpClient){
    this.newLanguage=new NewLanguageVM('','','','');
  }
  ngOnInit():void {

  try {

    //get app language code from server
    var user_language_code="";
   this.getAppLanguageCode().subscribe(
     resp=>{
      console.log(resp);
      //get launched app language code
        var current_language_code = window.location.pathname.split("/")[1];

        //check if user language code from server is known
        var availableCode = this.availableLanguageCodeTranslations.find(
          l=>l.code===resp
        );
        console.log(availableCode);
        if(resp!=="" && resp!==current_language_code)
        {
          //redirect
          window.location.href = `/${availableCode.code}`;
        }

     });


  } catch (error) {
    console.log(error);
    window.location.href = `/`;
  }

    //get the user configured languages
    this.getConfiguredLanguages(this.page_size);

    //get the levels parle, ecrit, comprehension
    this.http.get<Level[]>('http://localhost:50499/api/niveaux/parle').subscribe(
      resp=>{
          resp.forEach(np => {
            this.levels_parle.push(new Level(np.guid, np.name));
          });
      });

      this.http.get<Level[]>('http://localhost:50499/api/niveaux/ecrit').subscribe(
      resp=>{
          resp.forEach(ne => {
            this.levels_ecrit.push(new Level(ne.guid, ne.name));
          });
      });

    this.http.get<Level[]>('http://localhost:50499/api/niveaux/comprit').subscribe(
      resp=>{
          resp.forEach(nc => {
            this.levels_comprehension.push(new Level(nc.guid, nc.name));
          });
      });

  }

  //get application language from server
  private getAppLanguageCode(): Observable<string>{
    let appThis=this;
    let result= new Observable<string>(function(observer){
      appThis.http.get<AppLanguageGet>('http://localhost:50499/api/userlanguages/getAppLanguageCode').subscribe(
        resp=>
        {
          console.log(resp.code);
          observer.next(resp.code);
        });
        });
    return result;
  }

//changes app language to selected one
  changeAppLanguage(languageSelected:ConfiguredLanguage){
    if(languageSelected){   //if language exist
      //check if language code is known by app available translations
      var availableCode = this.availableLanguageCodeTranslations.find(
        l=>l.name.toLocaleLowerCase()=== languageSelected.language_name.toLocaleLowerCase()
      );
          console.log(languageSelected.language_name);
          console.log(languageSelected.guid);
          console.log(availableCode);
        if(availableCode){
          //set language change by api
          this.http.get<SetAppLanguageResult>('http://localhost:50499/api/userlanguages/setAppLanguage',{params:{"user_language_guid":`${languageSelected.guid}`}}).subscribe(
            resp =>{
                console.log(resp);
                if(resp.status.toString() === "200"){
                  window.location.href = `/${availableCode.code}`;
                }
            });
        }
    }
  }


  async onScroll(){
  //add new languages to list
  let page = 0;
  if(this.languages && this.total_configured_languages>this.languages.length)
    page=this.languages.length % this.page_size;
  page++;
  await this.getConfiguredLanguages(this.page_size, page);
}


private getConfiguredLanguages(page_size:number=2, page:number=0):ConfiguredLanguage[]  //retrieves languages
{
  this.http.get<UserLanguage_Get>('http://localhost:50499/api/userlanguages/paginate',{params:{'page_size':`${page_size}`,'page':`${page}`}})
    .subscribe(
      resp=>{

          this.total_configured_languages = resp.total_count;

          resp.data.forEach(element => {
          this.languages.push(new ConfiguredLanguage(
              element.guid,
              element.language_name,
              element.level_speak_name,
              element.level_write_name,
              element.level_understand_name,
              element.isAppLanguage,
              element.isCompulsoryLanguage
          ));
            //set
        });
        this.languages.sort(function(l,m){
          // console.log(l.language_name +" iscompulsory -> "+l.isCompulsoryLanguage);
          // console.log(m.language_name +" iscompulsory -> "+m.isCompulsoryLanguage);
        return (l.isCompulsoryLanguage && m.isCompulsoryLanguage) ? 0 : m.isCompulsoryLanguage ? 1 : -1;
      });
      }
    );

return this.languages;
}


deleteLanguage(languageToDelete:ConfiguredLanguage){    //deletes language from array
  if(languageToDelete !== undefined){
    //envoi requete de suppresion au serveur,
    //supprimer de la liste des langues courantes, la langue
    this.http.delete('http://localhost:50499/api/userlanguages/'+languageToDelete.guid).subscribe(resp=>
    {
      console.log(resp);
      //recupérer l'index de l'élément
      var index = this.languages.indexOf(languageToDelete, 0);
      if(index > -1){
        this.languages.splice(index,1);
      }
    });
  }
}


openDetail(content:any, language:ConfiguredLanguage){    //open modal for language details
  if(content!==null && content !== undefined && language !== null && language !== undefined)
  {
    this.isModalActive=true;
    this.languageToDetail = language;
    //launch modal
    this.modalService.open(content, {ariaLabelledBy:'modal-basic-title', windowClass:'custom-class'}).result.then((result)=>{
      this.closeResult = `Closed with: ${result}`;
      this.isModalActive=false;
    }, (reason)=>{
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
    });
  }
}

updateLanguageConfiguration(language:ConfiguredLanguage){
  //TODO
}

openCreate(content:any){    //open the create modal after updating/reseting objects and collections used
  if(content!==null && content !== undefined){

    //get not yet added languages
    //reset languages that can be configured
    if(this.languagesNames && this.languagesNames.length>0){
        var number_languages=this.languagesNames.length;
        this.languagesNames.splice(0,number_languages);
    }
    this.http.get<Language[]>('http://localhost:50499/api/languages/configurable').subscribe(resp=>{
    console.log(resp);
      resp.forEach(lang => {
        this.languagesNames.push(lang)
      });
    });

    this.newLanguage = new NewLanguageVM('','','','');  //reset property fields
    this.modalService.open(content, {ariaLabelledBy:'modal-basic-title', windowClass:'custom-class'}).result.then((result)=>{
      this.closeResult = `Closed with: ${result}`;
      console.log(this.closeResult);
      console.log(result);

    }, (reason)=>{
      this.closeResult = `Dismissed ${this.getDismissReason(reason)}`;
      console.log(this.closeResult);
    });
  }
}


saveNewLanguage(languageToSave:NewLanguageVM){    //save new language to array
  console.log(languageToSave);
  //send a new language creation to server, server returns a language view item to add to current languages
  this.http.post<ConfiguredLanguage>('http://localhost:50499/api/userlanguages', languageToSave).subscribe(
    resp => {
      console.log(resp);
      this.languages.push(new ConfiguredLanguage(
              resp.guid,
              resp.language_name,
              resp.level_speak_name,
              resp.level_write_name,
              resp.level_understand_name,
              resp.isAppLanguage,
              resp.isCompulsoryLanguage
      ));
      setTimeout(() => {
        this.modalService.dismissAll('language created');
      }, 100);
    });

}

private getDismissReason(reason: any){    //the reason the modal is dismissed
  this.isModalActive=false;
  console.log(reason);
}

}
