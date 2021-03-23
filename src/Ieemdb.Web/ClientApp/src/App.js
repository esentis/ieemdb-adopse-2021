import React, { Component } from 'react';
import AuthorizeRoute from './components/api-authorization/AuthorizeRoute';
import { Route } from 'react-router';
import ApiAuthorizationRoutes from './components/api-authorization/ApiAuthorizationRoutes';
import { ApplicationPaths } from './components/api-authorization/ApiAuthorizationConstants';

import {Container,Row} from 'react-bootstrap';

import LeftSide from './components/LeftSide'
import RightSide from './components/RightSide'

import './custom.css'

export default class App extends Component {
  static displayName = App.name;

  render () {
    return (
      <Container fluid>
            <AuthorizeRoute path='/fetch-data' component={FetchData} />
            <Route path={ApplicationPaths.ApiAuthorizationPrefix} component={ApiAuthorizationRoutes} />
            <Row>
              <LeftSide/>
              <RightSide/>
            </Row>
        </Container>
    );
  }
}
