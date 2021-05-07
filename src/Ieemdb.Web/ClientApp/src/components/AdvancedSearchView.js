import React,{useEffect, useState} from 'react'
import {Col} from 'react-bootstrap';
import '../Styles/AdvancedSearch.css'
import {useUpdatePage} from './GlobalContext';
import GenresSelect from './GenresSelect';
import RatingSelect from './RatingSelect';
import DateSelect from './DateSelect';
import {useHistory} from 'react-router-dom';
import Snackbar from '@material-ui/core/Snackbar';
import MuiAlert from '@material-ui/lab/Alert';


function AdvancedSearchView() {
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})

        //style for react-select
        const myStyles={
            control: styles => ({ ...styles, background:"none",border:"2px solid #D3D3D3",color:"black" }),
            option:styles=>({...styles,color:"black"}),
            colors:styles=>({primary25:"blue"}),
            placeholder:styles=>({color:"#D3D3D3"}),
            singleValue:styles=>({color:"#D3D3D3"})
        }

        const history=useHistory();

        //input states
        const [inputs,setInputs]=useState({MovieTitle:"",
                                        ActorName:"",
                                        DirectorName:"",
                                        WriterName:"",
                                        Duration:""})        
        const [Genres,setGenres]=useState("");
        const [Rating1,setRating1]=useState("");
        const [Rating2,setRating2]=useState("");
        const [Date1,setDate1]=useState("");
        const [Date2,setDate2]=useState("");
        const [RatingDisabled,setRatingDisabled]=useState(true);
        const [DateDisabled,setDateDisabled]=useState(true);
        const [ratingOptions,setRatingOptions]=useState("");
        const [DateOptions,setDateOptions]=useState("")
        const [RatingValue,setRatingValue]=useState("");
        const [DateValue,setDateValue]=useState("");
        const [errorMessage,setErrorMessage]=useState("");


        //Error SnackBar
        const [open,setOpen]=useState(false);
        function Alert(props) {
            return <MuiAlert elevation={6} variant="filled" {...props} />;
          }
        function OpenSnackBar(){setOpen(true)};
        function CloseSnackBar(){setOpen(false)};

        //Button Search
        function handleSearchClick(){
        if(inputs.MovieTitle!==""||inputs.ActorName!==""||inputs.DirectorName!==""||inputs.WriterName!==""||inputs.Duration!==""
        ||Genres!==""||Rating1!==""||Date1!=="")
        {
        if(Rating1 !== "" && Rating2 === ""){
            setErrorMessage("You must fill in the \"To\" Rating ")
            OpenSnackBar();

        }
        else if(Date1 !== "" && Date2 === ""){
            setErrorMessage("You must fill in the \"To\" Date ")
            OpenSnackBar();
        }else {
            history.push('/AdvancedSearchResults/MovieTitle='+inputs.MovieTitle+" ActorName="+inputs.ActorName+" DirectorName="+
            inputs.DirectorName+" WriterName="+inputs.WriterName+" Duration="+inputs.Duration+" Genres="+Genres+" FromRating="+Rating1+
            " ToRating="+Rating2+" FromDate="+Date1+" ToDate="+Date2)}
        }
        else{
        setErrorMessage("Υou must fill in at least one field")
        OpenSnackBar();                   }
        }

        //Functions for handling inputs
        function handleInputs(e){
           setInputs({...inputs,[e.target.name]:e.target.value}) 

        }

        function handleGenre(value){
            setGenres([value.map(obj=>obj.id)]);
            if(value.length===0){
                setGenres("")
            }}

        function handleRating1(value){
            addRatings(value.label);
            setRating1(value.label);
            setRatingDisabled(false);
            setRatingValue("")
            setRating2("")
        }

        function addRatings(arg){
            const AvailableRatings=[];
            for(var i=arg;i<=5;i++){
                AvailableRatings.push({value:i,label:i})
            }
            setRatingOptions(AvailableRatings);
        }

        function handleRating2(value){
            setRating2(value.label)
            setRatingValue(value);
        }

        function addDates(arg){
            const d=new Date();
            const AvailableDates=[];
            for(var i=arg;i<=d.getFullYear();i++){
                AvailableDates.push({value:i,label:i})
            }
            setDateOptions(AvailableDates);
        }

        function handleDate1(value){
            addDates(value.label);
            setDate1(value.label);
            setDateDisabled(false);
            setDateValue("");
            setDate2("");
        }

        function handleDate2(value){
            setDate2(value.label);
            setDateValue(value);
        }
          
    return (
       <Col className='column-right-AdvancedSearch'>
       <div className="AdvancedForm">
          <p><input placeholder="Movie Title" name={"MovieTitle"} onChange={handleInputs} ></input></p>
          <p><input placeholder="Actor Name" name={"ActorName"} onChange={handleInputs} value={inputs.ActorName} ></input></p>
          <p><input placeholder="Director Name" name={"DirectorName"} onChange={handleInputs} value={inputs.DirectorName}></input></p>
          <p><input placeholder="Writer Name" name={"WriterName"} onChange={handleInputs} value={inputs.WriterName}></input></p>
          <p><input type='number' placeholder="Duration(Minutes)" name={"Duration"} onChange={handleInputs} value={inputs.Duration}></input></p>
          <GenresSelect style={myStyles} onChange={handleGenre}  />
          <RatingSelect style={myStyles} onChange1={handleRating1} onChange2={handleRating2} isDisabled={RatingDisabled} options={ratingOptions} value={RatingValue}/>
          <DateSelect style={myStyles} onChange1={handleDate1} onChange2={handleDate2} isDisabled={DateDisabled} options={DateOptions} value={DateValue} />
          <p><button className="SearchButton" onClick={handleSearchClick}>Search</button></p> 
          <Snackbar open={open} autoHideDuration={6000} onClose={CloseSnackBar} anchorOrigin={{vertical:'bottom',horizontal:'right'}}  >
            <Alert onClose={CloseSnackBar} severity="error">
            {errorMessage}
        </Alert>
      </Snackbar></div>
       </Col>
    )
}

export default AdvancedSearchView;