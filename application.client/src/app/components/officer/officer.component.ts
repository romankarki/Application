import { Component } from '@angular/core';
import { OfficerServiceService } from '../../services/officer-service/officer-service.service';

@Component({
  selector: 'app-officer',
  standalone: true,
  imports: [],
  templateUrl: './officer.component.html',
  styleUrl: './officer.component.css'
})
export class OfficerComponent {

  constructor(private officerService:OfficerServiceService){}
  selectedFile: File | null = null;
  selectFile(event:any){
    const input = event.target as HTMLInputElement;
    if(input.files && input.files.length > 0){
      this.selectedFile = input.files[0];
    }
  }
  save(){
    if(this.selectedFile == null) {
      alert("Please Select a file first");
      return;
    }
    let data = new FormData();
    data.append("file", this.selectedFile);
    this.officerService.uploadOfficersData(data).subscribe({
      next:(res)=>{
        console.log("Response of upload is ", res);
        let rejectMessage = "";
        res?.rejectedRecords?.map((each:any)=>{
          rejectMessage += `${each?.identificationNumber} : ${each?.errorMessage}`+"\n";
        })
        let message = `total inserted : ${res?.totalInserted} \n
        total updated : ${res?.totalUpdated} \n
        total deleted : ${res?.totalDeleted} \n
        Rejected Records : \n
        ${rejectMessage}
        `
        alert(message);
        this.selectedFile = null;

      },
      error:(err)=>{
        alert("Something went wrong while uploading the file");
      }

    });

  }
}
