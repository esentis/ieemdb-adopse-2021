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
import DurationSelect from './DurationSelect';


function AdvancedSearchView() {
    const setPage=useUpdatePage();
    useEffect(() => {
        setPage("1")})

        //style for react-select
        const myStyles={
            control: styles => ({ ...styles, background:"none",border:"2px solid rgb(59, 94, 189);",color:"black" }),
            option:styles=>({...styles,color:"black"}),
            colors:styles=>({...styles,primary25:"blue"}),
            // placeholder:styles=>({color:"#282828"}),
            // singleValue:styles=>({color:"#282828"})
        }

        const history=useHistory();

        //input states
        const [inputs,setInputs]=useState({MovieTitle:"",
                                        ActorName:"",
                                        DirectorName:"",
                                        WriterName:""
                                        })        
        const [Genres,setGenres]=useState("");
        const [Rating1,setRating1]=useState("");
        const [Rating2,setRating2]=useState("");
        const [Date1,setDate1]=useState("");
        const [Date2,setDate2]=useState("");
        const [Duration1,setDuration1]=useState("");
        const [Duration2,setDuration2]=useState("");
        const [DurationOptions,setDurationOptions]=useState("");
        const [durationValue,setDurationValue]=useState("");
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

        useEffect(() => {
            var years=[];
            var d=new Date();
            for(var max=d.getFullYear(); max>=1900;max--){
                years.push({value:max,label:max})
        }
        const Ratings=[];
            for(var i=1;i<=5;i++){
            Ratings.push({value:i,label:i});
        }

        const duration=[];
        for(var i=20;i<=360;i+=10){
            duration.push({value:i,label:i});
        }
            setDurationOptions(duration)
            setRatingOptions(Ratings);
            setDateOptions(years);
        },[])

        //Button Search
        function handleSearchClick(){
        if(inputs.MovieTitle!==""||inputs.ActorName!==""||inputs.DirectorName!==""||inputs.WriterName!==""||Duration1!==""||Duration2!==""
        ||Genres!==""||Rating1!==""||Rating2!==""||Date1!==""||Date2!=="")
        {
            history.push('/AdvancedSearchResults/MovieTitle='+inputs.MovieTitle+" ActorName="+inputs.ActorName+" DirectorName="+
            inputs.DirectorName+" WriterName="+inputs.WriterName+" MinDuration="+Duration1+" MaxDuration="+Duration2+" Genres="+Genres+" FromRating="+Rating1+
            " ToRating="+Rating2+" FromDate="+Date1+" ToDate="+Date2)
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
            setGenres(value.map(obj=>obj.id));
            if(value.length===0){
                setGenres("")
            }}

        function handleRating1(value){
            addRatings(value.label);
            setRating1(value.label);
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
                AvailableDates.push({value:i,label:i});
            }
            setDateOptions(AvailableDates);
        }

        function addDuration(arg){
            const durations=[];
            for(var i=arg;i<=360;i+=10){
                durations.push({value:i,label:i});
            }
            setDurationOptions(durations);
        }

        function handleDate1(value){
            addDates(value.label);
            setDate1(value.label);
            setDateValue("");
            setDate2("");
        }

        function handleDate2(value){
            setDate2(value.label);
            setDateValue(value);
        }

        function handleDuration1(value){
            addDuration(value.label);
            setDuration1(value.label);
            setDurationValue("");
            setDuration2("");
            console.log()
        }

        function handleDuration2(value){
            setDuration2(value.label);
            setDurationValue(value);
        }
          
    return (
       <Col className='column-right-AdvancedSearch'>
       <div className="AdvancedForm">
          <p><input placeholder="Movie Title" name={"MovieTitle"} onChange={handleInputs} ></input></p>
          <p><input placeholder="Actor LastName" name={"ActorName"} onChange={handleInputs} value={inputs.ActorName} ></input></p>
          <p><input placeholder="Director LastName" name={"DirectorName"} onChange={handleInputs} value={inputs.DirectorName}></input></p>
          <p><input placeholder="Writer LastName" name={"WriterName"} onChange={handleInputs} value={inputs.WriterName}></input></p>
          <DurationSelect style={myStyles} onChange1={handleDuration1} onChange2={handleDuration2} options={DurationOptions} value={durationValue} />
          <p></p>
          <GenresSelect style={myStyles} onChange={handleGenre}  />
          <RatingSelect style={myStyles} onChange1={handleRating1} onChange2={handleRating2}  options={ratingOptions} value={RatingValue}/>
          <DateSelect style={myStyles} onChange1={handleDate1} onChange2={handleDate2}  options={DateOptions} value={DateValue} />
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