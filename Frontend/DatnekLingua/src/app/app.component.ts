import { Component, OnInit } from '@angular/core';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Language, NewLanguageVM } from './models';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'DatnekLingua';
  public newLanguage:NewLanguageVM;
  levelIndicators=[{guid:'1', name:'Bas'}, {guid:'2', name:'Courant'},{guid:'3', name:'Elevé'}];
  languagesNames=[{guid:'1', name:'Africaans'}, {guid:'2', name:'Yoruba'},{guid:'3', name:'Français'},{guid:'4', name:'Espagnol'}]
  isModalActive = false;
  scrollDistance=1;
  throttle=50;
  selector="search-results";
  languageToDetail:any;
  languages:Language[];
  closeResult: string="";


  constructor(private modalService: NgbModal){
    this.newLanguage=new NewLanguageVM('','','','');
    this.languages = [new Language('Français','Courant','Courant', 'Courant',true,true),
    new Language('Suedois','Courant', 'Bas','Courant'),
    new Language('Espagnol','Courant', 'Courant','Courant'),
    new Language('Italien','Courant', 'Courant','Courant'),
    new Language('Néerlandais', 'Courant','Courant', 'Courant', false, true)].sort(function(l,m){
      return (l.isCompulsoryLanguage === m.isCompulsoryLanguage) ? 0 : l.isCompulsoryLanguage ? -1 : 1;
    });   //liste of current languages
  }


private checkIfLanguageIsContained(language_name:string, current_languages:Language[]):boolean{   //check if language is in array
  if(current_languages!== null && current_languages !== undefined && current_languages.length > 0){
    for (let index = 0; index < current_languages.length; index++) {
      const element = current_languages[index];
      let name:String = element.nom;
      if(language_name === name)
      return true;
    }
  }
  return false;
}


languageSelection(languageItem:HTMLElement){   //selection of application language
  var classelmts = languageItem.className;
  //if it has active already as class pass, if not add active class to element and remove in siblings
  if(classelmts.indexOf("active")===-1){
    languageItem.className = classelmts + ' active';
    var sibs = this.getSiblings(languageItem);
    sibs.forEach(elmt=>{
        elmt.className = elmt.className.replace("active","");
    });
    //change application language to be implemented
  }
}


private getSiblings(element:HTMLElement): HTMLElement[]{    //get the element siblings
    // for collecting siblings
    let siblings:HTMLElement[] = [];
    // if no parent, return no sibling
    if(!element.parentNode) {
        return siblings;
    }
    // first child of the parent node
    let sibling  = element.parentNode.firstChild as HTMLElement;
    // collecting siblings
    while (sibling) {
        if (sibling.nodeType === 1 && sibling !== element) {
            siblings.push(sibling);
        }
        sibling = sibling.nextSibling as HTMLElement;
    }
    return siblings;
}


onScroll(){
  //add new languages to list
  var languagesRetrieved = this.getLanguages();
  languagesRetrieved.forEach(lang=>{
    this.languages['push'](lang);
  });
}


private getLanguages():Language[]   //retrieves languages
{
return [new Language('Chinois','Courant', 'Bas','Courant'),
        new Language('turc','Courant', 'Bas','Courant'),
        new Language('Bresilien','Courant', 'Bas','Courant')];
}


deleteLanguage(languageToDelete:Language){    //deletes language from array
  if(languageToDelete !== undefined){
    //envoi requete de suppresion au serveur,
    //supprimer de la liste des langues courantes, la langue
    //recupérer l'index de l'élément
    var index = this.languages.indexOf(languageToDelete, 0);
    if(index > -1){
      this.languages.splice(index,1);
    }
  }
}


openDetail(content:any, language:any){    //open modal for language details
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


openCreate(content:any){    //open the create modal after updating/reseting objects and collections used
  if(content!==null && content !== undefined){
    //get not yet added languages
    this.languagesNames=this.languagesNames.filter(n=>{
      //check if language is contained in current languages
      var checkResult =  !(this.checkIfLanguageIsContained(n.name, this.languages));//? false: true;
      console.log("Match result: " + checkResult);
      return (checkResult);
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
  //create a new language view item to add to current languages
  //get language name
  var languageToAdd:Language = new Language('', '', '',''); //nouvelle langue vide
  languageToAdd.nom = "" + (this.languagesNames.find(ln=>ln.guid===languageToSave.selected_name_id))?.name;
  languageToAdd.niveauParle = "" + this.levelIndicators.find(l=>l.guid === languageToSave.selected_talk_id)?.name
  languageToAdd.niveauEcrit = "" + this.levelIndicators.find(l=>l.guid === languageToSave.selected_write_id)?.name
  languageToAdd.niveauCompris = "" + this.levelIndicators.find(l=>l.guid === languageToSave.selected_understand_id)?.name
  //ajouter à la liste de langue disponible
  this.languages['push'](languageToAdd);
  //close modal
  setTimeout(() => {
    this.modalService.dismissAll('language created');
  }, 500);
}


private getDismissReason(reason: any){    //the reason the modal is dismissed
  this.isModalActive=false;
  console.log(reason);
}

}
