import React, { Component } from 'react';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import { Route } from 'react-router';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';
import {FetchData} from './components/FetchData';
import {Container,Row} from 'react-bootstrap';
import LeftSide from './components/LeftSide'
import RightSide from './components/RightSide'
import GlobalContextProvider from './components/GlobalContext'
import {BrowserRouter as Router} from 'react-router-dom';

import './Styles/custom.css'

export default class App extends Component{
  static displayName = App.name;
  static token;
  render () {
    return (
      <Container fluid>

              <AuthorizeRoute path='/fetch-data' component={FetchData} />
              <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
              <Row>
            <GlobalContextProvider>
              <Router>
              <LeftSide/>
              <RightSide />
        </Router>
        </GlobalContextProvider>
            </Row>
        </Container> 
    );
          }
        }
